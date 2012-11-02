using System;
using Wof.Controller;
using Wof.Model.Configuration;
using Wof.Model.Level.Common;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.Watercraft;
using Wof.Model.Level.Weapon;

namespace Wof.Model.Level.Planes
{
	 public enum AttackObject
    {
        Carrier,
        UserPlane,
        None
    }
	 
    public abstract class EnemyPlaneBase : Plane
    {
        /// <summary>
        /// Okreœla czas po jakim mo¿e nast¹piæ ponowny okrzyk bojowy
        /// </summary>
        private const float warCryTimerMin = 40.0f;

        /// <summary>
        /// Maksymalna odlegloœæ od samolotu gracza, na któr¹ mo¿e siê oddaliæ.
        /// </summary>
        private const float distanceFromUserPlane = 20;

        /// <summary>
        /// Bezpieczna odleg³oœæ (na osi Y) od samolotu gracza.
        /// </summary>
        private const float safeUserPlaneHeightDiff = 20;

        /// <summary>
        /// Maksymalna odlegloœæ od samolotów na lotniskowcu, na któr¹ mo¿e siê oddaliæ.
        /// </summary>
        private const float distanceFromStoragePlanes = 60;

        /// <summary>
        /// Maksymalny k¹t jaki mo¿e mieæ samolot wroga.
        /// </summary>
        private const float maxAngle = maxTurningAngle - float.Epsilon;

        /// <summary>
        /// Okreœla na jak¹ wysokoœæ nad samolot gracza bêdzie siê wznosi³/opada³ samolot wroga.
        /// </summary>
        private const float userPlaneHeightDiff = 1;

        /// <summary>
        /// pomocniczy licznik (czêstotliwoœæ okrzyków bojowych)
        /// </summary>
        private float warCryTimer;

        private PointD temp;

        /// <summary>
        /// Poprzednie po³o¿enia samolotu
        /// </summary>
        private float?[] interpolateSet;
        
        protected bool useInterpolation;
        
		public bool UseInterpolation {
			get {
        		
        		UpdateDebugInfo("UseInterpolation", useInterpolation);
        		return useInterpolation;
        	}
			set { 
        		UpdateDebugInfo("UseInterpolation", value);
        		useInterpolation = value;
        	}
		}

        /// <summary>
        /// Liczba punktów interpolacji
        /// </summary>      
        private int interpolationPoints = 5;

        private int lastInterpolationNull = -1;

        /// <summary>
        /// Czas jaki pozosta³, ¿eby móc wystrzeliæ kolejn¹ rakietê.
        /// </summary>
        private float timeToNextRocket = 0;

       

        private bool isAlarmDelivered;
        private float carrierDistance;
        private AttackObject attackObject;
        private float evadeLoopTime = 0.0f;
        private float evadeLoopTimeMax = 2.0f;
        private int evadeLoop = 0;
        private int evadeLoopMax = new Random().Next(4,6);
        private int evadeDirection = 1;
        private float lastAbsDistance = -1.0f;
        private bool isEvading = false;
        
		public bool IsEvading {
			get { return isEvading; }
		}
        private float randomDistance;

        protected void InitIterpolateSet() {
        	UseInterpolation = true;
        	interpolateSet = new float?[interpolationPoints];
            for (int i = 0; i < interpolateSet.Length; i++)
            {
                interpolateSet[i] = null;
            }        	
        }
        
        public EnemyPlaneBase(Level level, float width, float height, Planes.PlaneType planeType)
            : base(level, width, height, true, null, planeType)
        {
        	
            isEnemy = true;
            movementVector = new PointD((float)direction * (ConstantsObj as GameConsts.EnemyPlaneBase).Speed, 0);
            locationState = LocationState.Air;
            wheelsState = WheelsState.In;
            motorState = EngineState.Working;
            isAlarmDelivered = false;
            temp = new PointD(0, 0);

          	InitIterpolateSet();
        }

        /// <summary>
        /// Tworzy samolot z wylosawnym po³o¿eniem (któryœ z krañców planszy).
        /// </summary>
        /// <param name="level"></param>
        public EnemyPlaneBase(Level level, Planes.PlaneType planeType)
            : base(level, true, planeType)
        {
            StartPositionInfo info = new StartPositionInfo();
        	
            //wylosowanie pozycji
            Random r = new Random();
            int atEnd = r.Next(0, 2); //losuje 0 albo 1
            float endPos = (level.LevelTiles.Count)*LevelTile.TileWidth - 2.0f*width;

            float x;

            if ((level.MissionType == MissionType.Dogfight || level.MissionType == MissionType.Survival) && level.EnemyPlanesLeft == level.EnemyFightersPoolCount)
            {
                // pierwszy samolot jest blizej lotniskowca
                x = atEnd * endPos * 0.6f + (1 - atEnd) * endPos * 0.4f;
            }
            else
            {
                x = atEnd * endPos + (1 - atEnd) * endPos * 0.05f;
            }
           
            x += r.Next(-6, 6);
            
            // HARD!
            //x = 1350; GameConsts.UserPlane.Singleton.GodMode=true;
            	
            float y = r.Next(30, 40);
            info.Direction = atEnd == 0 ? Direction.Right : Direction.Left;
            info.EngineState = EngineState.Working;
            info.WheelsState  = WheelsState.In;
            info.PositionType = StartPositionType.Airborne;
            info.Position = new PointD(x,y);
            info.Speed = GetConsts().Speed*0.01f*r.Next(90, 111);
            bounds = new Quadrangle(new PointD(x, y), width, height);
            this.startPositionInfo = info;
            Init();

            attackObject = AttackObject.None;
            //	StartEngine();
            level.OnEnemyPlaneFromTheSide(!(atEnd == 1));
            temp = new PointD(0, 0);

            InitIterpolateSet();

            //Console.WriteLine("Enemy plane from the " + (atEnd==1?"right.":"left."));
        }

        public AttackObject AttackObject
        {
            get { return attackObject; }
            set { attackObject = value; }
        }

        /// <summary>
        /// Sprawdza, czy samolot powinien zawróciæ - tzn. czy nie jest za daleko od samolotu gracza
        /// </summary>
        private bool ShouldTurnRound
        {
            get
            {
                if (ShouldBeChasingUserPlane) //leci za samolotem
                {
                    string debugStr = "";
                   
                    if(!IsAfterUserPlane || ShouldSteerUp)
                    {
                        UpdateDebugInfo("ShouldTurnRoundCheck-IsAfterUserPlane", IsAfterUserPlane.ToString());
                        UpdateDebugInfo("ShouldTurnRoundCheck-ShouldSteerUp", ShouldSteerUp.ToString()); 
                        return false;
                    }
                    float diff = Center.X - level.UserPlane.Center.X;
                    if (isChasedBy(level.UserPlane))
                    {
                        // na przeciwko
                        
                        if (Math.Abs(diff) > 3.5f * distanceFromUserPlane)
                        {
                            UpdateDebugInfo("ShouldTurnRoundCheck-ChasedByUser-distance", true.ToString()); 
                            // Console.WriteLine("OK, zawrot kiedy leca na przeciw");
                            return true;
                        } else
                        {
                            UpdateDebugInfo("ShouldTurnRoundCheck-ChasedByUser-distance", false.ToString()); 
                            return false;
                        }
                    } else
                    {
                        UpdateDebugInfo("ShouldTurnRoundCheck-PlayerMiniety", true.ToString()); 
                        // minely sie
                        return (Math.Abs(diff) > distanceFromUserPlane);
                    }
                   
                     
                }
                else //atakuje samoloty na lotniskowcu
                {
                    if (!ArePlanesOnCarrierBehind ||
                        ShouldSteerUp ||
                        (Math.Abs(Center.X - PlanesOnAircraftPos.X) < distanceFromStoragePlanes))
                    {
                        UpdateDebugInfo("ShouldTurnRoundCheck-Carrier", false.ToString()); 
                        return false;
                    }

                    if( DistanceToClosestEnemyPlane() > 20)
                    {
                        UpdateDebugInfo("ShouldTurnRoundCheck-DistanceToClosestEnemyPlane>20", true.ToString()); 
                        return true;
                    }else
                    {
                        UpdateDebugInfo("ShouldTurnRoundCheck-DistanceToClosestEnemyPlane>20", false.ToString()); 
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Sprawdza, czy samolot min¹³ w³aœnie samolot gracza
        /// </summary>
        private bool IsPassingUserPlane
        {
            get
            {
                float currentDistanceToUserPlane = Center.X - level.UserPlane.Center.X;
                float absDistance = Mogre.Math.Abs(currentDistanceToUserPlane);
                float distanceFactor = 6.5f * Mogre.Math.Abs(level.UserPlane.MovementVector.X) / minFlyingSpeed;
                float absYDistance = Mogre.Math.Abs(Center.Y - level.UserPlane.Center.Y);

                bool result = (absDistance < lastAbsDistance && absDistance < distanceFactor && absYDistance < 1.5f * distanceFactor);

                lastAbsDistance = absDistance;
                return result;
            }
        }

        /// <summary>
        /// Sprawdza, czy znajduje siê za samolotem gracza.
        /// </summary>
        private bool IsAfterUserPlane
        {
            get
            {
                //return (direction == Direction.Right && Center.X > level.UserPlane.Center.X) ||
                //         (direction == Direction.Left && Center.X < level.UserPlane.Center.X);
                return (direction == Direction.Right && bounds.LeftMostX > level.UserPlane.Bounds.RightMostX) ||
                       (direction == Direction.Left && bounds.RightMostX < level.UserPlane.Bounds.LeftMostX);
            }
        }

        /// <summary>
        /// Sprawdza czy samolot znajduje siê za samolotami na lotniskowcu
        /// </summary>
        private bool ArePlanesOnCarrierBehind
        {
            get
            {
                return (direction == Direction.Right && Center.X > PlanesOnAircraftPos.X) ||
                       (direction == Direction.Left && Center.X < PlanesOnAircraftPos.X);
            }
        }

        /// <summary>
        /// Sprawdza czy samolot powinien zwiêkszyæ pu³ap, ¿eby nie spaœæ poni¿ej minimalnego.
        /// </summary>
        private bool ShouldSteerUp
        {
            get
            {
                ShipTile st = this.GetNearestShipCrashThreat();
                if(st != null)
                {	
                    
                    if(YDistanceToTile(st) < 10*height)
                    {
                    	
                    	//Console.WriteLine(this.Name+"-"+this.planeType+"; ydiff" + YDistanceToTile(st) + " ->" + (15*height));
             
                        return true;
                    }
                }
            	
                if (bounds.LowestY <= GetConsts().MinPitch){ //czy ju¿ nie jest za nisko
                	Console.WriteLine(this.Name+"-"+this.planeType+", Jestem za nisko!");
                    return true;
            	}
                if (RelativeAngle >= 0)
                    return false;
                // if (this.Center.Y > GameConsts.UserPlane.Singleton.MaxHeight * 0.9f) return false;

                //czas po którym samolot dotrze do mininalnego pu³apu
                float timeToGetTheLowestPitch = (bounds.LowestY - GetConsts().MinPitch)/
                                                Math.Abs(movementVector.Y);
                //czas po którym samolot "wyprostuje siê"
                float timeToGetToHorizonatal = Math.Abs(RelativeAngle)/rotateStep;
                return timeToGetToHorizonatal >= 0.5*timeToGetTheLowestPitch;
            }
        }

        /// <summary>
        /// Zwraca czy samolot wroga ma lecieæ w kierunku samolotu gracza.
        /// </summary>
        private bool ShouldBeChasingUserPlane
        {
            get
            {
                return !level.UserPlane.IsOnAircraftCarrier && //nie atakuje samolotu na lotniskowcu
                       (
                           level.StoragePlanes.Count <= 0 || //nie ma samolotow na lotniskowcu
                           weaponManager.RocketCount <= 0 || //nie ma ju¿ rakiet
                           (Math.Abs(Center.X - level.UserPlane.Center.X) < Math.Abs(Center.X - PlanesOnAircraftPos.X))
                       );
            }
        }

        /// <summary>
        /// Zwraca pozycjê samolotów pozosta³ych na lotniskowcu.
        /// Jeœli nie ma samolotów zwraca pozycjê RestoreTile
        /// </summary>
        private PointD PlanesOnAircraftPos
        {
            get
            {
                if (level.StoragePlanes.Count <= 0 || level.UserPlane.IsOnAircraftCarrier)
                    //gdy nie ma samolotów zwraca pozycjê RestoreTile
                    return level.Carrier.GetRestoreAmunitionPosition();
                else
                    return
                        new PointD(level.StoragePlanes[0].Bounds.Center.X + randomDistance,
                                   level.StoragePlanes[0].Bounds.Center.Y);
                //return new PointD(level.StoragePlanes[level.StoragePlanes.Count - 1].Bounds.Center.X - 2*LevelTile.Width, level.StoragePlanes[level.StoragePlanes.Count - 1].Bounds.Center.Y); 
            }
        }

        /// <summary>
        /// Zwraca czy samolot mo¿e trafiæ któryœ z samolotów na lotniskowcu.
        /// </summary>
        private bool CanHitStoragePlanes
        {
            get
            {
                for (int i = 0; i < level.StoragePlanes.Count; i++)
                    if (
                        level.StoragePlanes[i].PlaneState == PlaneState.Intact &&
                        Math.Abs(level.StoragePlanes[i].Center.X - Center.X) <
                        GetConsts().AttackStoragePlaneDistance &&                        
                        Rocket.CanHitEnemyPlane(this, level.StoragePlanes[i], 5 , false) != MissileBase.CollisionDirectionLocation.NONE)
                        return true;
                return false;
                //RelativeAngle < -maxAngle/2; 
            }
        }

        /// <summary>
        /// Sprawdza, czy trzeba w danym momencie tak sterowaæ samolotem, ¿eby nie rozbi³
        /// siê o samolot gracza.
        /// </summary>
        private float UserPlaneCrashThreat
        {
            get
            {
                Plane p = level.UserPlane;
                
                              
                if(IsAfterUserPlane) return 0;
                
                /* PointD cut = MiddleLine.Intersect(p.MiddleLine);
                   
                if(cut == null) return false;
                            
                float distToCut = (cut - this.bounds.Center).EuclidesLength;*/
               
                float distX = Math.Abs(Center.X - p.Center.X);
                float distY = Math.Abs(Center.Y - p.Center.Y);
                
                float str = - (distX / GetConsts().SafeUserPlaneDistance)   + 2; // 2*safe -> 0; 0 -> 2.0
           
                if(direction == p.Direction && distX < GetConsts().SafeUserPlaneDistance && (distY < safeUserPlaneHeightDiff))
                {
                    //Console.WriteLine("Distance warning 1 "+ str);
                	
                    return 1.0f * str;               	
                }
                
                if(direction != p.Direction && distX < 1.5f *GetConsts().SafeUserPlaneDistance && 1.5f * distY < safeUserPlaneHeightDiff)
                {
                    //Console.WriteLine("Distance warning 2 "+ str);
                    return 1.5f * str;    
                }
                            
               
                if((p.MovementVector - movementVector).EuclidesLength > 10) {
                    if(direction != p.Direction  && distX < 2.0f * GetConsts().SafeUserPlaneDistance && distY < 2.0f *safeUserPlaneHeightDiff)
                    {
                        //Console.WriteLine("Movement warning 1 "+ str);
                        return 2.0f * str;    
                    }
                }
                
                return 0;
            }
        }

      
        protected void EvadePlaneWhenChased(Plane p, float scaleFactor, float maxEvadeAngle) 
        {        	
            isEvading = true;
        	
            if(ShouldSteerUp) 
            {	
            	isEvading = false;
                //Console.Write("Zawracam bo juz unik nie ma sensu - walne w ziemie");
                TurnRound((Direction)(-1 * (int)direction), TurnType.Airborne);
                return;        		
            }
            
            if(Math.Abs(Center.Y - p.Center.Y) > safeUserPlaneHeightDiff * 1.5f )
            {
            	isEvading = false;
                //Console.Write("Zawracam bo juz unik nie ma sensu - i tak samolot gracza jest na innym pulapie");
                TurnRound((Direction)(-1 * (int)direction), TurnType.Airborne);
                return;               	
            }
                      
            evadeLoopTime += scaleFactor;
            if(evadeLoopTime >= evadeLoopTimeMax)
            {            	
                evadeLoop++;
                evadeLoopTime = 0;
                evadeDirection *= -1;            	
                evadeLoopTimeMax = new Random().Next(1,3) * 1.7f; // maksymalny czas w jednej 
            }
            
            if(evadeLoop >= evadeLoopMax) 
            {
            	isEvading = false;
                evadeLoop = 0;   
                evadeLoopMax = new Random().Next(4, 7);  // 4 do 6 zmian kierunku          	
                TurnRound((Direction)(-1 * (int)direction), TurnType.Airborne);            	
            }
            
            
            // uciekaj
            if(Speed < GetConsts().Speed * 1.4f)
            {
                Speed += 0.8f * scaleFactor;
            }
            
            if(evadeDirection == 1)
            {
                if(RelativeAngle < maxEvadeAngle)
                {                	
                	UpdateDebugInfo("RotateUp-Evade",true);
                    float angleDiff = 0.75f * (maxEvadeAngle - RelativeAngle) / maxEvadeAngle;
                    RotateUp(angleDiff * scaleFactor * rotateStep );
                     
                }
            }
            
            if(evadeDirection == -1)
            {
                if(RelativeAngle > -maxEvadeAngle)
                {                	 
                	UpdateDebugInfo("RotateDown-Evade",true);
                    float angleDiff =  0.65f * (RelativeAngle + maxEvadeAngle) / maxEvadeAngle;	
                    RotateDown(angleDiff * scaleFactor * rotateStep);
                   
                }
            }
            
            return;
          
            /*
        	float yDiff =  1 + Math.Abs(Center.Y - targetY);
	        float yDiffNorm = (float)Math.Log10(yDiff) * 0.5f;
	        
		   
	        if (Center.Y > targetY + userPlaneHeightDiff && !shouldPullup)
	        {
	            if(RelativeAngle > -maxEvadeAngle) 
	            {	            	
	            	float angleDiff = (RelativeAngle + maxEvadeAngle) / maxEvadeAngle;					        	
					 //Console.WriteLine("DOWN PITCH: " + yDiff + " normalized: " + yDiffNorm+ " rotDiff: "+angleDiff);
	               
	            	RotateDown(angleDiff * scaleFactor * rotateStep * yDiffNorm);
	            }
	        }
	        else //czy ma lecieæ w górê
	        {
	            if (Center.Y < targetY - userPlaneHeightDiff)
	            {	            	
	                if(RelativeAngle < maxEvadeAngle)
	                {
	                	float angleDiff = (maxEvadeAngle - RelativeAngle) / maxEvadeAngle;
	                	
	                	// Console.WriteLine("UP PITCH: " + yDiff + " normalized: " + yDiffNorm+ " rotDiff: "+angleDiff);
	                	Console.WriteLine(" - faktycznie o :"+(angleDiff * scaleFactor * rotateStep * yDiffNorm));
	                	RotateUp(angleDiff * scaleFactor * rotateStep * yDiffNorm);
	                }
	            }
	        }*/
        }

        public override void Move(float time, float timeUnit)
        {
            EndDebugIteration();
            
            float  angleBefore = movementVector.Angle;
           
    		UpdateDebugInfo("1Bounds.Angle",bounds.Angle);
    		UpdateDebugInfo("2movementVector.Angle",movementVector.SignX==1 ?  movementVector.Angle : Math.PI + movementVector.Angle );
    		UpdateDebugInfo("3turningVector.Angle",turningVector.Angle);

    	
            
            timeToNextRocket -= time;
            timeToNextRocket = Math.Max(0, timeToNextRocket);
            //dodane tymczasowo, bo po wgraniu planszy pierwszy komunikat odœwie¿enia przychodzi 
            //z bardzo du¿¹ wartoœci¹ time
            //   if (time > timeUnit/10)
            //       return;
            if (planeState == PlaneState.Crashed)
            {
                MoveAfterCrash(time, timeUnit);
                return;
            }
            float scaleFactor = time/timeUnit;
            
            if (planePaused)
            {
                scaleFactor *= 0.1f;
            }
              
            warCryTimer += scaleFactor;

            UpdateDamage(scaleFactor); //wyciek oleju
            HeightLimit(time, timeUnit);
            VerticalBoundsLimit(time, timeUnit);

            if (planeState == PlaneState.Destroyed) //jeœli zniszczony to tylko spada
            {
                FallDown(time, timeUnit, GlideType.destroyed);
            }
            else
            {
                //czy ma ju¿ zawróciæ
                if (locationState == LocationState.Air)
                {
                    if (IsPassingUserPlane)
                    {
                        level.Controller.OnPlanePass(level.UserPlane);
                    }
                    if (ShouldTurnRound)
                    {
                        UpdateDebugInfo("ShouldTurnRound", true.ToString());

                        if (RelativeAngle != 0)
                        {
                            UpdateDebugInfo("ShouldTurnRound-SteerToHorizon", true.ToString());
                            //jeœli musi podci¹gn¹æ lot, to najpierw wyrównuje do poziomu
                            UseInterpolation = false;
                            SteerToHorizon(scaleFactor);
                        }
                        else
                        {
                        	UseInterpolation = true;
                            UpdateDebugInfo("ShouldTurnRound-TurnRound!", true.ToString());
                            TurnRound((Direction) (-1*(int) direction), TurnType.Airborne);
                        }

                        randomDistance = new Random().Next(-GetConsts().StoragePlaneDistanceFault,
                                                           GetConsts().StoragePlaneDistanceFault) +
                                         (0.1f) * new Random().Next(-10, 10);
                    }
                    else {
                    	
                        UseInterpolation = !isEvading;
                    	
                        //nie musi zawracaæ - kontynuuj lot
                        if(isChasedBy(level.UserPlane)){
                            // minal juz samolot gracza wiec niech unika ostrzalu
                    		
                            //	Console.WriteLine(this.Name + ": Unikam ostrzalu");
                           
                            EvadePlaneWhenChased(level.UserPlane, scaleFactor, maxAngle * 0.6f);
                    		
                        }else {                        	
                        	isEvading = false;
                            UpdateDebugInfo("ChangePitch", true.ToString()); 
                            ChangePitch(scaleFactor);
                        }
                    }
                }
                //zmiana wektora ruchu przy zawracaniu
                if (locationState == LocationState.AirTurningRound && isChangingDirection)
                {
                    turningTimeLeft -= time;
                    movementVector = -Mogre.Math.Cos((turningTimeLeft / turningTime) * Mogre.Math.PI) * turningVector;
                    // Speed = MinFlyingSpeed; - wylaczone by Adam (samolot dziwnie zawraca³ ;)
                    // Console.WriteLine(this.Name + " " + movementVector.ToString());

                }
                if (locationState == LocationState.Air && planeState != PlaneState.Crashed &&
                    motorState == EngineState.SwitchedOff)
                    FallDown(time, timeUnit, GlideType.glider);

                //atak samolotu gracza
                if (ShouldBeChasingUserPlane)
                {
                    AttackUserPlane(level.UserPlane, scaleFactor);
                }
            }
            // Console.WriteLine(temp + "   -   " + movementVector);

            // zmiana by Adam (wyg³adzenie ruchu)
            if (movementVector.EuclidesLength > 0)
            {
                temp.X = movementVector.X;
                if (isChangingDirection)
                    temp.Y = movementVector.Y;
                else
                    temp.Y = movementVector.Y*0.7f;
            }
            else
            {
                temp.X /= 2.0f;
                temp.Y /= 2.0f;
            }


            //Interpolacja by Kamil
            
            if(UseInterpolation)
            {
	            if (lastInterpolationNull != (interpolateSet.Length - 1))
	            {
	                for (int i = 0; i < interpolateSet.Length; i++)
	                {
	                    if (interpolateSet[i] == null)
	                    {
	                        lastInterpolationNull = i;
	                        break;
	                    }
	                }
	            }
	
	            for (int i = 1; i <= lastInterpolationNull; i++)
	            {
	                interpolateSet[i] = interpolateSet[i - 1];
	            }
	
	            interpolateSet[0] =  (movementVector.Angle - angleBefore) * 1.0f;
	
	
	            float finalValue = 0;
	            for (int i = 0; i <= lastInterpolationNull; i++)
	            {
	                //finalValue.X += interpolateSet[i].X;
	               // finalValue.Y += interpolateSet[i].Y;
	               finalValue += interpolateSet[i].Value;
	            }
	            //finalValue.X /= lastInterpolationNull + 1;
	            //finalValue.Y /= lastInterpolationNull + 1;
	           finalValue /= lastInterpolationNull + 1;	
	           
		       rotateValue = finalValue * 0.5f;
	           UpdatePlaneAngle(scaleFactor);
	           
            }
           
            //Koniec interpolacji by Kamil

            bounds.Move(scaleFactor * temp);
      
           
               
         //  rotateValue = (movementVector.Angle - angleBefore) * 1.0f;
         
            // zmiana by Adam

            if (IsEngineWorking)
            {
                airscrewSpeed = minAirscrewSpeed + (int) Mogre.Math.Abs((int) (15f*movementVector.X));
            }

           
        }


       
       


        /// <summary>
        /// Zmienia pu³ap samolotu.
        /// </summary>
        /// <param name="scaleFactor"></param>
        private void ChangePitch(float scaleFactor)
        {
            //jeœli za bardzo leci w dó³ - podci¹gam lot. Przede wszystkim ma siê nie rozbiæ
            if (ShouldSteerUp)
            {
                //Console.WriteLine(this.Name + ": ShouldSteerUp");
                UpdateDebugInfo("ShouldSteerUp", true.ToString()); 
      			if(RelativeAngle < maxAngle)
                {
                	Rotate((float) direction*scaleFactor*rotateStep);
      			}
                return;
            }
            EnemyPlaneBase closestEnemyPlane;

            if (ShouldBeChasingUserPlane)
            {
                UpdateDebugInfo("ShouldBeChasingUserPlane", true.ToString()); 
      
                if (attackObject != AttackObject.UserPlane)
                {
                    attackObject = AttackObject.UserPlane;
                    isAlarmDelivered = false;
                }


                if ((closestEnemyPlane = GetNearestEnemyPlaneCrashThreat()) != null) //czy ma omin¹æ inne samoloty
                {
                    if (closestEnemyPlane.PlaneState != PlaneState.Crashed)
                    {
                        UpdateDebugInfo("AvoidEnemyPlaneCrash", true.ToString()); 
                        AvoidEnemyPlaneCrash(scaleFactor, closestEnemyPlane);
                    }
                }
                else
                {
                    float str = UserPlaneCrashThreat;
                    if (str > 0) //czy ma omin¹æ gracza               
                    {
                        //Trace.WriteLine("AVOIDING!!!");
                        UpdateDebugInfo("AvoidUserPlaneCrash", true.ToString()); 
                        AvoidUserPlaneCrash(scaleFactor * str);
                    }                
                    else
                    {
	                   
                        //czy ma lecieæ w dó³
                        float yDiff =  1 + Math.Abs(Center.Y - level.UserPlane.Center.Y);
                        float yDiffNorm = (float)Math.Log10(yDiff) * 0.5f;
	
	                   
	
                        if (Center.Y > level.UserPlane.Center.Y + userPlaneHeightDiff)
                        {
                            if(RelativeAngle > -maxAngle) 
                            {
	                        	
                                float angleDiff = (RelativeAngle + maxAngle) / maxAngle;

								UpdateDebugInfo("Rotate Down", true.ToString());       	
                                //Console.WriteLine("DOWN PITCH: " + yDiff + " normalized: " + yDiffNorm+ " rotDiff: "+angleDiff);
	                           
                                RotateDown(angleDiff * scaleFactor * rotateStep * yDiffNorm);
                            }
                        }
                        else //czy ma lecieæ w górê
                        {
                            if (Center.Y < level.UserPlane.Center.Y - userPlaneHeightDiff)
                            {
                                if(RelativeAngle < maxAngle)
                                {
                                    float angleDiff = (maxAngle - RelativeAngle) / maxAngle;
								 
                                    // Console.WriteLine("UP PITCH: " + yDiff + " normalized: " + yDiffNorm+ " rotDiff: "+angleDiff);
                                    UpdateDebugInfo("Rotate Up", true.ToString()); 
                                    RotateUp(angleDiff * scaleFactor * rotateStep * yDiffNorm);
                                }
                            }
                            else //czy ma prostowaæ samolot 
                                if (Math.Abs(RelativeAngle) >= 0)
                                {
                                    UpdateDebugInfo("Horizon", true.ToString()); 
                                    //  Console.WriteLine("HORIZON PITCH: " + yDiff + " normalized: " + yDiffNorm);
                                    SteerToHorizon(scaleFactor * yDiffNorm * Math.Abs(RelativeAngle) / maxAngle);
	                            
                                }
	                                
                        }
                    }
                }
            }
            else
            {
                carrierDistance = Math.Abs(Center.X - level.Carrier.GetRestoreAmunitionPosition().X);

                if (PlanesOnAircraftPos.CompareTo(level.Carrier.GetRestoreAmunitionPosition()) == 0)
                {
                    isAlarmDelivered = false;
                    UpdateDebugInfo("AttackObject-NONE", true.ToString()); 
                    attackObject = AttackObject.None;
                }
                else
                {
                    UpdateDebugInfo("AttackObject-CARRIER", true.ToString()); 
                    attackObject = AttackObject.Carrier;
                }
                // 
                if (attackObject == AttackObject.Carrier && !isAlarmDelivered &&
                    carrierDistance < GetConsts().CarrierDistanceAlarm)
                {
                    UpdateDebugInfo("AttackObject-CARRIER-attacking", true.ToString()); 
                    level.Controller.OnEnemyAttacksCarrier();
                    isAlarmDelivered = true;
                }

                if ((closestEnemyPlane = GetNearestEnemyPlaneCrashThreat()) != null) //czy ma omin¹æ inne samoloty
                {
                    if (closestEnemyPlane.PlaneState != PlaneState.Crashed)
                        AvoidEnemyPlaneCrash(scaleFactor, closestEnemyPlane);
                }
                else                	
                {
                    //najpierw ma nie zderzyæ siê z graczem
               
                    float str = UserPlaneCrashThreat;
                    if (str > 0) //czy ma omin¹æ gracza               
                    {
                        //Trace.WriteLine("AVOIDING!!!");
                        AvoidUserPlaneCrash(scaleFactor * str);
                    }  
                    else
                    {
                        if (ArePlanesOnCarrierBehind) //jeœli ju¿ min¹³ cel, to zwiêkszam pu³ap
                        {
                            UpdateDebugInfo("AttackObject-CARRIER-PlanesOnCarrierBehind", true.ToString()); 
                            if (RelativeAngle < maxAngle)
                            {
                                RotateUp(scaleFactor*rotateStep);
                            }
                        }
                        else
                        {
                            if (CanHitStoragePlanes)
                            {
                                UpdateDebugInfo("AttackObject-CARRIER-AttackStoragePlanes", true.ToString()); 
                                AttackStoragePlanes();
                            }
                            else
                            {
                                // dziób do do³u jeœli atakuje coœ lub jest b. blisko lotniskowca
                                if ((carrierDistance < 0.2f * GetConsts().CarrierDistanceAlarm ||
                                     attackObject != AttackObject.None))
                                {
                                	if(EngineConfig.Difficulty != EngineConfig.DifficultyLevel.Easy) {
                                		weaponManager.FireAtAngle(Angle, WeaponType.Gun, this.locationState == LocationState.AirTurningRound,  MissileBase.CollisionDirectionLocation.FORWARD);
                                	}
                                	
                                	if(RelativeAngle > -maxAngle){
                                    	UpdateDebugInfo("AttackObject-CARRIER-RotateDown", true.ToString()); 
                                    	RotateDown(scaleFactor*rotateStep);
                                	}

                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Steruje samolotem tak, ¿eby wróci³ do poziomu.
        /// </summary>
        /// <param name="scaleFactor"></param>
        private void SteerToHorizon(float scaleFactor)
        {
        	float angleAbs = Mogre.Math.Abs(RelativeAngle);
        	
        	float step= rotateStep*scaleFactor;	
        	step = step * angleAbs * 2.0f;
        	UpdateDebugInfo("SteerToHorizon-step", step); 
        	UpdateDebugInfo("SteerToHorizon-angleAbs", angleAbs); 
        	        
        	if(step < 0.2f * scaleFactor) {
        		step = 0.2f * scaleFactor;
        		UpdateDebugInfo("SteerToHorizon-scaleFactor", scaleFactor); 
        	}        	
        	step = Math.Min(step, angleAbs);
        	
        	
        	UpdateDebugInfo("SteerToHorizon-realStep", step); 
          
        	
            //Console.WriteLine(this.Name + ": Steer to horizon");
            Rotate(-1.0f*(float) direction*Math.Sign(RelativeAngle)*step);
        }
        
       /* private void SteerTo(float scaleFactor, float angle)
        {
        	
            //Console.WriteLine(this.Name + ": Steer to horizon");
            Rotate(-1.0f*(float) direction*Math.Sign(RelativeAngle)*
                   Math.Min(rotateStep*scaleFactor, Mogre.Math.Abs(RelativeAngle)));
        }*/

        /// <summary>
        /// Wystrzeliwuje rakietê blokuj¹c zbyt czêste strza³y.
        /// </summary>
        private void FireRocket()
        {
            if (timeToNextRocket <= 0)
            {
                timeToNextRocket = GetConsts().NextRocketInterval;
                weaponManager.Fire(WeaponType.Rocket);
            }
        }

       

        /// <summary>
        /// Próbuje zaatkaowaæ samolot gracza. Najpierw sprawdza mo¿liwoœæ ataku rakiet¹
        /// a póŸniej dzia³kiem.
        /// </summary>
        private void AttackUserPlane(Plane userPlane, float scaleFactor)
        {
            //return; //chwilowo do testów
            //sprawdzam czy samolot nie jest za daleko, ¿eby atakowaæ

            UpdateDebugInfo("AttackUserPlane", true.ToString()); 
            
            // staraj sie dogonic samolot gracza
            if (IsTurnedTowardsUserPlane(userPlane) && Speed < GetConsts().Speed * 1.4f)
            {
                Speed += 0.8f * scaleFactor;
            }
         
            if (!IsTurnedTowardsUserPlane(userPlane) && Speed > minFlyingSpeed)
            {
                Speed -= 0.8f * scaleFactor;
            }


            if (Math.Abs(Center.X - level.UserPlane.Center.X) > GetConsts().ViewRange)
            {
                return;
            }

            UpdateDebugInfo("AttackUserPlane-Distance", true.ToString()); 
			

            if (this.weaponManager.RocketCount > 0 && Rocket.CanHitEnemyPlane(this, level.UserPlane, 100 - GetConsts().Accuracy, false) != MissileBase.CollisionDirectionLocation.NONE) //najpierw próbuje strzeliæ rakiet¹
            {
                if(warCryTimer > warCryTimerMin)
                {
                    level.Controller.OnWarCry(this);
                    warCryTimer = 0;
                }
                FireRocket();
            }           
			else  
            {
                UpdateDebugInfo("AttackUserPlane-GunAiming", true.ToString()); 
			
				MissileBase.CollisionDirectionLocation coll = GunBullet.CanHitEnemyPlane(this, level.UserPlane, 100 - GetConsts().Accuracy, this.HasBiDirectionalGun);
				if(coll != MissileBase.CollisionDirectionLocation.NONE)
				{
	                if (warCryTimer > warCryTimerMin)
	                {
	                    level.Controller.OnWarCry(this);
	                    warCryTimer = 0;
	                }
	                //Console.WriteLine(this.Name + ": AttackUserPlane -> can hit");
	                weaponManager.FireAtAngle(Angle, WeaponType.Gun, this.locationState == LocationState.AirTurningRound, coll);
				}
            }
               
        }

        /// <summary>
        /// Próbuje zaatakowaæ samoloty na lotniskowcu. Nie bêdzie strzela³, jeœli gracz jest 
        /// na lotniskowcu.
        /// </summary>
        private void AttackStoragePlanes()
        {
            //Console.WriteLine(this.Name + ": AttackStoragePlanes");
            
            
            // na easy samolot wroga nie atakuje storage planes
            if(EngineConfig.Difficulty == EngineConfig.DifficultyLevel.Easy)
            {
                return;
            }
        	
            if(EngineConfig.Difficulty == EngineConfig.DifficultyLevel.Medium && Mogre.Math.RangeRandom(0.0f, 1.0f) > 0.99f)
            {
                return; // na medium, czasem atak nie jest wykonywany
            }
        	
            // na hardzie atak jest zawsze
        	
            if (warCryTimer > warCryTimerMin)
            {
                level.Controller.OnWarCry(this);
                warCryTimer = 0;
            }
            
            
            // atakuj nawet jesli samolot gracza jest na lotniskowcu ale tylko na hardzie i nie zawsze
            if ((EngineConfig.Difficulty == EngineConfig.DifficultyLevel.Hard && Mogre.Math.RangeRandom(0.0f, 1.0f) > 0.95f) || 
                !level.UserPlane.IsOnAircraftCarrier)
                FireRocket();
        }

        /// <summary>
        /// Steruje tak samolotem, ¿eby nie wpaœæ na samolot gracza
        /// </summary>
        /// <param name="scaleFactor"></param>
        private void AvoidUserPlaneCrash(float scaleFactor)
        {
            //Console.WriteLine(this.Name + ": AvoidUserPlaneCrash");
            
            AvoidCrash(scaleFactor, level.UserPlane);
        }

        private void AvoidCrash(float scaleFactor, Plane p)
        {
            UpdateDebugInfo("Avoid crash", true.ToString());       
            float diff = Center.Y - p.Center.Y;
            float diffAbs = Math.Abs(diff);
            if (diffAbs > safeUserPlaneHeightDiff)
            {
                diffAbs = safeUserPlaneHeightDiff;
            }

            float yFactor = (safeUserPlaneHeightDiff - diffAbs) / safeUserPlaneHeightDiff;
            
            float directionFactor = isTurnedTowardsFaceOf(p) ? 2.0f : 1.0f;


            if (diff > 0)
            {
                if (RelativeAngle < maxAngle)
                {
                    // Console.WriteLine("Strength up: " + strength);
                    RotateUp(directionFactor * yFactor * scaleFactor * 1.25f * rotateStep);
                }
            }
            else
            {
                if (RelativeAngle > -maxAngle)
                {
                    // Console.WriteLine("Strength down: " + strength);
                    RotateDown(directionFactor * yFactor * scaleFactor * 1.25f * rotateStep);
                }
            }
        }

        /// <summary>
        /// Steruje tak samolotem, ¿eby nie wpaœæ na inny wrogi samolot
        /// </summary>
        /// <param name="scaleFactor"></param>
        /// <param name="ep"></param>
        private void AvoidEnemyPlaneCrash(float scaleFactor, Plane ep)
        {
            //Console.WriteLine(this.Name + ": AvoidEnemyPlaneCrash");
            
            AvoidCrash(scaleFactor, ep);
        }

        protected bool isTurnedTowardsFaceOf(IObject2D obj)
        {
            if(direction == obj.Direction) return false;
            
            if(direction == Model.Level.Direction.Right)
            {
                return obj.Bounds.RightMostX > bounds.LeftMostX;
            }

            if (direction == Model.Level.Direction.Left)
            {
                return obj.Bounds.LeftMostX < bounds.RightMostX;
            }

            return false;
        }

        protected bool isChasedBy(IObject2D obj)
        {
            if (direction != obj.Direction) return false;

            if (direction == Model.Level.Direction.Right)
            {
                return obj.Center.X < Center.X;
            }

            if (direction == Model.Level.Direction.Left)
            {
                return obj.Center.X > Center.X;
            }

            return false;
        }

        /// <summary>
        /// Sprawdza, czy znajduje siê za samolotem gracza.
        /// </summary>
        private bool IsTurnedTowardsUserPlane(Plane userPlane)
        {
            return (direction == Direction.Right && bounds.LeftMostX < userPlane.Bounds.RightMostX) ||
                   (direction == Direction.Left && bounds.RightMostX > userPlane.Bounds.LeftMostX);
       
        }

        /// <summary>
        /// Zwraca ship tile ktory jest najblizej i jednoczesnie jest zagrodzeniem z uwagi na odleglosc
        /// </summary>
        /// <returns></returns>
        private ShipTile GetNearestShipCrashThreat()
        {
        	
            float dist = float.MaxValue;
            float temp;
            int index = -1;
            ShipTile st;
            for (int i = 0; i < level.ShipsList.Count; i++)
            {
                st = (ShipTile)(level.ShipsList[i]);
                float x = XDistanceToTile(st);
                float y = YDistanceToTile(st);
                if ( x != -1 && 
                     y != -1 &&
                     x < width * 30 &&
                     y < height * 10
                    )
                {
                    temp = x+y;
                    if (dist > temp)
                    {
                        dist = temp;
                        index = i;
                    }
                }
            }
            if (index != -1) return level.ShipsList[index] as ShipTile;
            return null;
        	
        }
  
        public new Wof.Model.Configuration.GameConsts.EnemyPlaneBase GetConsts()
		{
			return constantsObj as Wof.Model.Configuration.GameConsts.EnemyPlaneBase;
		}
        
        /// <summary>
        /// Zwraca najbli¿szy wrogi samolot gracza, który jest BLI¯EJ ni¿ minimalna bezpieczna odleg³oœæ, lec¹cy naprzeciw bie¿¹cego samolotu. NULL, gdy nie ma samolotu spe³niaj¹cego te warunki
        /// </summary>
        /// <returns></returns>
        private EnemyPlaneBase GetNearestEnemyPlaneCrashThreat()
        {
            float dist = float.MaxValue;
            float temp;
            int index = -1;
            Plane ep;
            for (int i = 0; i < level.EnemyPlanes.Count; i++)
            {
                ep = level.EnemyPlanes[i];

                // testowo wy³¹czony warunek
                /* 
             if (direction == ep.Direction)
                {
                    continue;
                }*/
                if (ep.Equals(this)) continue;

                if (
                    XDistanceToPlane(ep) < GetConsts().SafeUserPlaneDistance &&
                    YDistanceToPlane(ep) < safeUserPlaneHeightDiff
                    )
                {
                    temp = DistanceToPlane(ep);
                    if (dist > temp)
                    {
                        dist = temp;
                        index = i;
                    }
                }
            }
            if (index != -1) return level.EnemyPlanes[index] as EnemyPlaneBase;
            return null;
        }

        private void UpdateDamage(float scaleFactor)
        {
            if (planeState == PlaneState.Damaged) oil -= scaleFactor * OilLeak;
       
            if (planeState != PlaneState.Destroyed && planeState != PlaneState.Crashed && oil <= 0)
                OutOfPetrolOrOil(scaleFactor);
        }
    }
}