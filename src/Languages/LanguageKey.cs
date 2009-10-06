/*
 * Copyright 2008 Adam Witczak, Jakub Tężycki, Kamil Sławiński, Tomasz Bilski, Emil Hornung, Michał Ziober
 *
 * This file is part of Wings Of Fury 2.
 * 
 * Freeware Licence Agreement
 * 
 * This licence agreement only applies to the free version of this software.
 * Terms and Conditions
 * 
 * BY DOWNLOADING, INSTALLING, USING, TRANSMITTING, DISTRIBUTING OR COPYING THIS SOFTWARE ("THE SOFTWARE"), YOU AGREE TO THE TERMS OF THIS AGREEMENT (INCLUDING THE SOFTWARE LICENCE AND DISCLAIMER OF WARRANTY) WITH WINGSOFFURY2.COM THE OWNER OF ALL RIGHTS IN RESPECT OF THE SOFTWARE.
 * 
 * PLEASE READ THIS DOCUMENT CAREFULLY BEFORE USING THE SOFTWARE.
 *  
 * IF YOU DO NOT AGREE TO ANY OF THE TERMS OF THIS LICENCE THEN DO NOT DOWNLOAD, INSTALL, USE, TRANSMIT, DISTRIBUTE OR COPY THE SOFTWARE.
 * 
 * THIS DOCUMENT CONSTITUES A LICENCE TO USE THE SOFTWARE ON THE TERMS AND CONDITIONS APPEARING BELOW.
 * 
 * The Software is licensed to you without charge for use only upon the terms of this licence, and WINGSOFFURY2.COM reserves all rights not expressly granted to you. WINGSOFFURY2.COM retains ownership of all copies of the Software.
 * 1. Licence
 * 
 * You may use the Software without charge.
 *  
 * You may distribute exact copies of the Software to anyone.
 * 2. Restrictions
 * 
 * WINGSOFFURY2.COM reserves the right to revoke the above distribution right at any time, for any or no reason.
 *  
 * YOU MAY NOT MODIFY, ADAPT, TRANSLATE, RENT, LEASE, LOAN, SELL, REQUEST DONATIONS OR CREATE DERIVATE WORKS BASED UPON THE SOFTWARE OR ANY PART THEREOF.
 * 
 * The Software contains trade secrets and to protect them you may not decompile, reverse engineer, disassemble or otherwise reduce the Software to a humanly perceivable form. You agree not to divulge, directly or indirectly, until such trade secrets cease to be confidential, for any reason not your own fault.
 * 3. Termination
 * 
 * This licence is effective until terminated. The Licence will terminate automatically without notice from WINGSOFFURY2.COM if you fail to comply with any provision of this Licence. Upon termination you must destroy the Software and all copies thereof. You may terminate this Licence at any time by destroying the Software and all copies thereof. Upon termination of this licence for any reason you shall continue to be bound by the provisions of Section 2 above. Termination will be without prejudice to any rights WINGSOFFURY2.COM may have as a result of this agreement.
 * 4. Disclaimer of Warranty, Limitation of Remedies
 * 
 * TO THE FULL EXTENT PERMITTED BY LAW, WINGSOFFURY2.COM HEREBY EXCLUDES ALL CONDITIONS AND WARRANTIES, WHETHER IMPOSED BY STATUTE OR BY OPERATION OF LAW OR OTHERWISE, NOT EXPRESSLY SET OUT HEREIN. THE SOFTWARE, AND ALL ACCOMPANYING FILES, DATA AND MATERIALS ARE DISTRIBUTED "AS IS" AND WITH NO WARRANTIES OF ANY KIND, WHETHER EXPRESS OR IMPLIED. WINGSOFFURY2.COM DOES NOT WARRANT, GUARANTEE OR MAKE ANY REPRESENTATIONS REGARDING THE USE, OR THE RESULTS OF THE USE, OF THE SOFTWARE WITH RESPECT TO ITS CORRECTNESS, ACCURACY, RELIABILITY, CURRENTNESS OR OTHERWISE. THE ENTIRE RISK OF USING THE SOFTWARE IS ASSUMED BY YOU. WINGSOFFURY2.COM MAKES NO EXPRESS OR IMPLIED WARRANTIES OR CONDITIONS INCLUDING, WITHOUT LIMITATION, THE WARRANTIES OF MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE WITH RESPECT TO THE SOFTWARE. NO ORAL OR WRITTEN INFORMATION OR ADVICE GIVEN BY WINGSOFFURY2.COM, IT'S DISTRIBUTORS, AGENTS OR EMPLOYEES SHALL CREATE A WARRANTY, AND YOU MAY NOT RELY ON ANY SUCH INFORMATION OR ADVICE.
 * 
 * IMPORTANT NOTE: Nothing in this Agreement is intended or shall be construed as excluding or modifying any statutory rights, warranties or conditions which by virtue of any national or state Fair Trading, Trade Practices or other such consumer legislation may not be modified or excluded. If permitted by such legislation, however, WINGSOFFURY2.COM' liability for any breach of any such warranty or condition shall be and is hereby limited to the supply of the Software licensed hereunder again as WINGSOFFURY2.COM at its sole discretion may determine to be necessary to correct the said breach.
 * 
 * IN NO EVENT SHALL WINGSOFFURY2.COM BE LIABLE FOR ANY SPECIAL, INCIDENTAL, INDIRECT OR CONSEQUENTIAL DAMAGES (INCLUDING, WITHOUT LIMITATION, DAMAGES FOR LOSS OF BUSINESS PROFITS, BUSINESS INTERRUPTION, AND THE LOSS OF BUSINESS INFORMATION OR COMPUTER PROGRAMS), EVEN IF WINGSOFFURY2.COM OR ANY WINGSOFFURY2.COM REPRESENTATIVE HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES. IN ADDITION, IN NO EVENT DOES WINGSOFFURY2.COM AUTHORISE YOU TO USE THE SOFTWARE IN SITUATIONS WHERE FAILURE OF THE SOFTWARE TO PERFORM CAN REASONABLY BE EXPECTED TO RESULT IN A PHYSICAL INJURY, OR IN LOSS OF LIFE. ANY SUCH USE BY YOU IS ENTIRELY AT YOUR OWN RISK, AND YOU AGREE TO HOLD WINGSOFFURY2.COM HARMLESS FROM ANY CLAIMS OR LOSSES RELATING TO SUCH UNAUTHORISED USE.
 * 5. General
 * 
 * All rights of any kind in the Software which are not expressly granted in this Agreement are entirely and exclusively reserved to and by WINGSOFFURY2.COM.
 * 
 * 
 */

using Wof.Controller;

namespace Wof.Languages
{
    /// <summary>
    /// Klasa zawiera klucze do pobierania
    /// slow ze slownika slow.
    /// </summary>
    public static class LanguageKey
    {
        #region ---A---

      
        public const string AddingCompositors = @"AddingCompositors";
        public const string AccelerateBreakTurn = @"AccelerateBreakTurn";
        public const string Alive = @"Alive";
        public const string AllEnemyPlanesDestroyedLandOnCarrierAndPressX =
            @"AllEnemyPlanesDestroyedLandOnCarrierAndPressX";
        public const string AllEnemyShipsDestroyedLandOnCarrierAndPressX =
            @"AllEnemyShipsDestroyedLandOnCarrierAndPressX";
        public const string AllEnemySoldiersEliminatedLandOnTheCarrierAndPressX =
            @"AllEnemySoldiersEliminatedLandOnTheCarrierAndPressX";

 		public const string Altitude = @"Altitude";
        public const string Antyaliasing = @"Antialiasing";
        public const string AreYouTryingToReachStarsCaptain = @"AreYouTryingToReachStarsCaptain";
        public const string Assassination = @"Assassination";
        public const string AverageFPS = @"AverageFPS";

        #endregion

        #region ---B---
       
        public const string Back = @"Back";
        public const string BestFPS = @"BestFPS";
        public const string Blood = @"Blood";
        public const string BombingRun = @"BombingRun";
        public const string Bombs = @"Bombs";
        public const string BombsAccuracy = @"BombsAccuracy";
        public const string BombsDropped = @"BombsDropped";

        
        public const string Bloom = @"Bloom";
        public const string BulletTime = @"BulletTime"; 
        

        #endregion

        #region ---C---
        public const string Camera = @"Camera";
        public const string CameraChanged = @"CameraChanged";
        public const string ChangeOptionMessage1 = @"ChangeOptionMessage1";
        public const string ChangeOptionMessage2 = @"ChangeOptionMessage2";
        public const string CheckForUpdates = @"CheckForUpdates";
        public const string ChooseAntialiasingMode = @"ChooseAntialiasingMode";
        public const string ChooseDifficulty = @"ChooseDifficulty";
        public const string ChooseLevelOfDetail = @"ChooseLevelOfDetail";
        public const string ChooseVideoMode = @"ChooseVideoMode";
        public const string Congratulations = @"Congratulations";
        public const string CommunityTranslations = @"CommunityTranslations";
        public const string CompletedLevels = @"CompletedLevels";
        public const string CreatingGameObjects = @"CreatingGameObjects";
        public const string CreatingInput = @"CreatingInput";
        public const string CreatingScene = @"CreatingScene";
        public const string CreatingTheRootObject = @"CreatingTheRootObject";
        public const string CreatorOfOriginalWingsOfFury = @"CreatorOfOriginalWingsOfFury";
        public const string Credits = @"Credits";
        public const string Controls = @"Controls";
        public const string CoreTeam = @"CoreTeam";
        public const string CurrentFPS = @"CurrentFPS";

        #endregion

        #region ---D---

        public const string Details = @"Details";
        public const string Difficulty = @"Difficulty";
        public const string Dogfight = @"Dogfight";
        public const string Donate = "Donate";
        public const string DonateMessagePart1 = @"DonateMessagePart1";
        public const string DonateMessagePart2 = @"DonateMessagePart2";
        public const string DonateMessagePart3 = @"DonateMessagePart3";
        public const string DonateMessagePart4 = @"DonateMessagePart4";
        public const string DonateMessagePart5 = @"DonateMessagePart5";

        #endregion

        #region ---E---

        public const string EnableBloom = @"EnableBloom";
        public const string EnableSound = @"EnableSound";
        public const string EnableVSync = @"EnableVSync";
        public const string EnemyGenerals = @"EnemyGenerals";  
        public const string EnemyInstallationsAreBothSideCaptain = @"EnemyInstallationsAreBothSideCaptain";
        public const string EnemyInstallationsAreLeftCaptain = @"EnemyInstallationsAreLeftCaptain";
        public const string EnemyInstallationsAreRightCaptain = @"EnemyInstallationsAreRightCaptain";
        public const string EnemyInstallationDamaged = @"EnemyInstallationDamaged";
        public const string EnemyInstallationDestroyed = @"EnemyInstallationDestroyed";
        public const string EnemyPlaneDownIRepeat = @"EnemyPlaneDownIRepeat";
        public const string EnemyPlaneApproachingFromTheLeft = @"EnemyPlaneApproachingFromTheLeft";
        public const string EnemyPlaneApproachingFromTheRight = @"EnemyPlaneApproachingFromTheRight";
        public const string EnemyPlanesLeft = @"EnemyPlanesLeft";
        public const string EnemyShipsLeft = @"EnemyShipsLeft";
        public const string EnemySoldiersLeft = @"EnemySoldiersLeft";
        public const string Engine = @"Engine";
        public const string EnterYourName = @"EnterYourName";
        public const string ExitToMenu = @"ExitToMenu";

        #endregion

        #region ---G---

        public const string GameOver = @"GameOver";
        public const string Gear = @"Gear";
        public const string Graphics = @"Graphics";
        public const string Gun = @"Gun";
        public const string GunAccuracy = @"GunAccuracy";
        public const string GunShellsFired = @"GunShellsFired";
        

        #endregion

        #region ---H---

        public const string HallOfFame = @"HallOfFame";
        public const string High = @"High";
        public const string Highscores = @"Highscores";
        public const string HydraxWater = @"HydraxWater";
        

        #endregion

        #region ---I---

        public const string InitializingSound = @"InitializingSound";
        public const string InverseKeys = @"InverseKeys";

        #endregion

        #region ---K---

        public const string KeyboardInfo1 = @"KeyboardInfo1";
        public const string KeyboardInfo2 = @"KeyboardInfo2";
        public const string KillAllEnemySoldiers = @"KillAllEnemySoldiers";

        #endregion

        #region ---L---

        public const string LandOnTheCarrierAndPressXToRefuel = @"LandOnTheCarrierAndPressXToRefuel";
        public const string LandOnTheCarrierAndPressXToRepair = @"LandOnTheCarrierAndPressXToRepair";
        public const string LanguageID = @"LanguageID";
        public const string Languages = @"Languages";
        public const string Level = @"Level";
        public const string LevelCompleted = @"LevelCompleted";
        public const string LevelStats = @"LevelStats";
        public const string LoadingResources = @"LoadingResources";
        public const string Low = @"Low";
        public const string LowFuelWarning = @"LowFuelWarning";
        public const string LowOilWarning = @"LowOilWarning";

        #endregion

        #region ---M---

        public const string Medium = @"Medium";
        public const string MissionType = @"MissionType";
        

        #endregion

        #region ---N---

        public const string Naval = @"Naval";
        public const string Neutralized = @"Neutralized";
        public const string Next = @"Next";
        public const string NewGame = @"NewGame";
        public const string No = @"No";
        public const string None = @"None";
        

        #endregion

        #region ---O---

        public const string OhNoEnemySoldiersAreRebuildingBunker = @"OhNoEnemySoldiersAreRebuildingBunker";
        public const string OK = @"OK";
        public const string Options = @"Options";

        #endregion

        #region ---P---

        public const string Pause = @"Pause";
        public const string Pitch = @"Pitch";
        public const string PlanesDestroyed = @"PlanesDestroyed";
        public const string Poland = @"Poland";
        public const string PressEToTurnOnTheEngine = @"PressEToTurnOnTheEngine";
        public const string PressLeftUpToTakeOff = @"PressLeftUpToTakeOff";
        public const string PressXToChangeAmmo = @"PressXToChangeAmmo";
        
        public const string Previous = @"Previous";

        #endregion

        #region ---Q---

        public const string Quit = @"Quit";

        #endregion

        #region ---R---
        public const string RearmEndMission = @"RearmEndMission";
        public const string Resume = @"Resume";
        public const string RetractYourLandingGearWithG = @"RetractYourLandingGearWithG";
        public const string Rockets = @"Rockets";
        public const string RocketsFired = @"RocketsFired";
        public const string RocketsAccuracy = @"RocketsAccuracy";
     
        #endregion

        #region ---S---

        public const string ScreenshotWrittenTo = @"ScreenshotWrittenTo";
        public const string SelectAmmunition = @"SelectAmmunition";
        public const string SetupingResources = @"SetupingResources";
        public const string Shadows = @"Shadows";        
        public const string Sound = @"Sound";
        public const string SpecialThanksToSteveWaldo = @"SpecialThanksToSteveWaldo";
        public const string Spin = @"Spin";
        public const string StartFrom = @"StartFrom";
        public const string SupportTeam = @"SupportTeam";

        #endregion

        #region ---T---
        public const string Target = @"Target";
        public const string TargetNeutralizedLandOnCarrierAndPressX = @"TargetNeutralizedLandOnCarrierAndPressX";
        public const string Torpedoes = @"Torpedoes";
        public const string TorpedoTooHigh = @"TorpedoTooHigh";
        public const string TorpedoTooLongTravelDistance = @"TorpedoLongTravelDistance";
        public const string TriangleCount = @"TriangleCount";
        public const string Tutorial = @"Tutorial";

        #endregion

        #region ---V---

        public const string VideoMode = @"VideoMode";
        public const string VideoOptions = @"VideoOptions";
        public const string VSync = @"VSync";

        #endregion

        #region ---W---

        public const string WARNINGProtectTheCarrierFromEnemyPlane = @"WARNINGProtectTheCarrierFromEnemyPlane";
        public const string WeAreNotLeavingYetCaptain = @"WeAreNotLeavingYetCaptain";
        public const string WorstFPS = @"WorstFPS";

        #endregion

        #region ---Y---

        public const string Yes = @"Yes";
        public const string YouWonTheGame = @"YouWonTheGame";

        #endregion

        #region ---Z---

        public const string Zoomin = @"Zoomin";
        public const string Zoomout = @"Zoomout";

        #endregion


        
    }
}