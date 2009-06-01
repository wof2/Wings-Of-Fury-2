﻿/*
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

using System;
using System.Reflection;
using MOIS;
using System.Text;
using Wof.Misc;

namespace Wof.Controller.Input.Keyboard
{
    /// <summary>
    /// Mapa klawiszy odpowiadajaca odpowiednim zadaniom.
    /// </summary>
    public class KeyMap : IniFileConfiguration<KeyMap>
    {
        #region Private Constructor

        private KeyMap() : base("KeyMap") 
        {
        
        }
        
        #endregion

        #region Singleton

        /// <summary>
        /// Instancja klasy
        /// </summary>
        public static readonly KeyMap Instance = (new KeyMap()).Value;

        #endregion
        
        protected string GetString(string key, string defaultValue)
        {
        	try
        	{
        		string s = GetString(key);
        		if(s.Length == 0) return defaultValue;
        		return s;
        	}
        	catch(Exception)
        	{
        		return defaultValue;
        	}
        	
        }
        
        private KeyCode GetKeyCode(string iniKey, string defaultValue)
        {
        	return (KeyCode)KeyCode.Parse(typeof(KeyCode), GetString(iniKey, defaultValue));
        }
        
        private MOIS.Keyboard.Modifier GetModifier(string iniKey, string defaultValue)
        {
        	return (MOIS.Keyboard.Modifier)KeyCode.Parse(typeof(MOIS.Keyboard.Modifier), GetString(iniKey, defaultValue));
        }
        
        public override string ToString()
        {
        	StringBuilder sb = new StringBuilder();        	
        	sb.AppendLine("KeyMap object:");
        	sb.AppendLine("_altFire:" + _altFire);
        	sb.AppendLine("_gunFire:" + _gunFire);
        	sb.AppendLine("_up:" + _up);
        	sb.AppendLine("_down:" + _down);
        	sb.AppendLine("_left:" + _left);
        	sb.AppendLine("_right:" + _right);
        	sb.AppendLine("_bulletTimeEffect:" + _bulletTimeEffect);
        	sb.AppendLine("_enter:" + _enter);
        	sb.AppendLine("_back:" + _back);
        	sb.AppendLine("_gear:" + _gear);
        	sb.AppendLine("_camera:" + _camera);
        	sb.AppendLine("_engine:" + _engine);
        	sb.AppendLine("_spin:" + _spin);
        	return sb.ToString();
        }
        
        public override KeyMap Value
        {
        	get 
        	{
        		KeyMap k = new KeyMap();        		
        		k._altFire = GetKeyCode("_altFire", "KC_X");
        		k._gunFire = GetKeyCode("_gunFire", "KC_Z");
        		k._up = GetKeyCode("_up", "KC_UP");
        		k._down = GetKeyCode("_down", "KC_DOWN");
        		k._left = GetKeyCode("_left", "KC_LEFT");
        		k._right = GetKeyCode("_right", "KC_RIGHT");        	
        		k._bulletTimeEffect = GetKeyCode("_bulletTimeEffect", "KC_BACK");
        		k._enter = GetKeyCode("_enter", "KC_RETURN");
        		k._back = GetKeyCode("_back", "KC_ESCAPE");
        		k._gear = GetKeyCode("_gear", "KC_G");
        		k._camera = GetKeyCode("_camera", "KC_C");
        		k._engine = GetKeyCode("_engine", "KC_E");
        		
        		try
        		{
        			k._spin = GetModifier("_spin", "Ctrl");
        		}
        		catch
        		{
        			k._spin = GetKeyCode("_spin", "KC_S");
        		}
        		
        		
        		return k;
        	}
        	set 
        	{
        		WriteString("_altFire", _altFire.ToString());
        		WriteString("_gunFire", _gunFire.ToString());
        		WriteString("_up", _up.ToString());
        		WriteString("_down", _down.ToString());
        		WriteString("_left", _left.ToString());
        		WriteString("_right", _right.ToString());
        	
        		WriteString("_bulletTimeEffect", _bulletTimeEffect.ToString());
        		WriteString("_enter", _enter.ToString());
        		WriteString("_back", _back.ToString());        		
        		WriteString("_gear", _gear.ToString());
        		WriteString("_camera", _camera.ToString());
        		WriteString("_engine", _engine.ToString());
        		WriteString("_spin", _spin.ToString());
        	}
        	
        }

        #region Key Code

        private KeyCode _gunFire = KeyCode.KC_Z;

        /// <summary>
        /// Pobiera lub ustawia kod klawisza, ktory odpowiada za strzelania z dzialka samolotu.
        /// </summary>
        public KeyCode GunFire
        {
            get { return _gunFire; }
            set { _gunFire = value; }
        }
        
        
        private KeyCode _up;

        /// <summary>
        /// Pobiera lub ustawia kod klawisza, ktory odpowiada za strzelania z dzialka samolotu.
        /// </summary>
        public KeyCode Up
        {
            get { return _up; }
            set { _up = value; }
        }
        
        
        private KeyCode _down;

        /// <summary>
        /// Pobiera lub ustawia kod klawisza, ktory odpowiada za nacisniecie strzalki do dołu
        /// </summary>
        public KeyCode Down
        {
            get { return _down; }
            set { _down = value; }
        }
        
        
        private KeyCode _left;

        /// <summary>
        /// Pobiera lub ustawia kod klawisza, ktory odpowiada za nacisniecie strzalki do lewo
        /// </summary>
        public KeyCode Left
        {
            get { return _left; }
            set { _left = value; }
        }
        
        
        private KeyCode _right;

        /// <summary>
        /// Pobiera lub ustawia kod klawisza, ktory odpowiada za nacisniecie strzalki do lewo
        /// </summary>
        public KeyCode Right
        {
            get { return _right; }
            set { _right = value; }
        }
        

        //---------------------------------------------------

        private KeyCode _altFire;

        /// <summary>
        /// Pobiera lub ustawia kod klawisza, ktory odpowiada za strzelanie rakietami, zrzucaniem bomb i torped, itp.
        /// </summary>
        public KeyCode AltFire
        {
            get { return _altFire; }
            set { _altFire = value; }
        }

        //---------------------------------------------------

        private KeyCode _bulletTimeEffect;

        /// <summary>
        /// Pobiera lub ustawia kod klawisza, ktory odpowiada za wlaczenie efektu BulletTime.
        /// </summary>
        public KeyCode BulletTimeEffect
        {
            get { return _bulletTimeEffect; }
            set { _bulletTimeEffect = value; }
        }
        
        
        
        
        private KeyCode _enter;

        /// <summary>
        /// Pobiera lub ustawia kod klawisza, ktory odpowiada za enter.
        /// </summary>
        public KeyCode Enter
        {
            get { return _enter; }
            set { _enter = value; }
        }
        
        
        
        private KeyCode _back;

        /// <summary>
        /// Pobiera lub ustawia kod klawisza, ktory odpowiada za esc.
        /// </summary>
        public KeyCode Escape
        {
            get { return _back; }
            set { _back = value; }
        }
        
        
        private KeyCode _gear;
       
        /// <summary>
        /// Pobiera lub ustawia kod klawisza, ktory odpowiada za nacisniecie gear
        /// </summary>
        public KeyCode Gear
        {
            get { return _gear; }
            set { _gear = value; }
        }
        
        
        private KeyCode _camera;

        /// <summary>
        /// Pobiera lub ustawia kod klawisza, ktory odpowiada za zmiane kamery.
        /// </summary>
        public KeyCode Camera
        {
            get { return _camera; }
            set { _camera = value; }
        }
        
        
        private KeyCode _engine;
       
        /// <summary>
        /// Pobiera lub ustawia kod klawisza, ktory odpowiada za nacisniecie strzalki do lewo
        /// </summary>
        public KeyCode Engine
        {
            get { return _engine; }
            set { _engine = value; }
        }
        
        
        private object _spin ;
       
        /// <summary>
        /// Pobiera lub ustawia kod klawisza, ktory odpowiada za nacisniecie spina
        /// </summary>
        public object Spin
        {
            get { return _spin; }
            set { _spin = value; }
        }

        #endregion
        
        #region Public Static Methods

        /// <summary>
        /// Metoda sprawdza czy dany KeyCode jest zajety przez jakas funkcjonalnosc.
        /// </summary>
        /// <param name="presentKeyCode">Aktualny KeyCode dla danej funkcjonalnosci.</param>
        /// <param name="newKeyCode">Nowy KeyCode dla danej funkcjonalnosci.</param>
        /// <returns>Jesli oba parametry sa rowne, to nie ma konfliktu - zwroci false.
        /// Jesli KeyCode podany w drugim parametrze jest zajety przez jakas funkjonalnosc, to metoda zwroci true.
        /// W przeciwnym przypadku false.</returns>
        public static bool CheckKeyCodeConflict(KeyCode presentKeyCode, KeyCode newKeyCode)
        {
            if (presentKeyCode != newKeyCode)
            {
                KeyCode tmpKeyCode;
                PropertyInfo[] properties = KeyMap.Instance.GetType().GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    tmpKeyCode = (KeyCode)properties[i].GetValue(Instance, null);
                    if (tmpKeyCode == newKeyCode)
                        return true;
                }
            }
            return false;
        }
        
        public static string GetName(object keyCodeOrModifier)
        {
        	if(keyCodeOrModifier is KeyCode)
        	{
        		return ((KeyCode)keyCodeOrModifier).ToString().Substring(3);
        	}else
        	if(keyCodeOrModifier is MOIS.Keyboard.Modifier)
        	{
        		return ((MOIS.Keyboard.Modifier)keyCodeOrModifier).ToString();
        	} else
        	{
        		return "";
        	}
        			
        	
        }

        #endregion
    }
}