using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Wof.Controller;
using Wof.Languages;

namespace Wof.Tools
{
    /// <summary>
    /// Tool służący do sprawdzania sumy kontrolnej plików
    /// </summary>
    public class GenerateFontRanges
    {
    	public const string outputFile = "GenerateFontRanges.log";
    	
    	/// <summary>
    	/// Adds standard latin range: 33-126
    	/// 
    	/// </summary>
    	public const bool addStandardRange = true;
    
        private static void Main(string[] args)
        {
            // string outputFile;
           // if (args.Length < 1)
            {
                Console.WriteLine("Usage: GenerateFontRanges.exe [languageCode]");             
                Console.WriteLine("Generates font ranges used by the given language code.");
                Console.WriteLine("If languageCode is omitted all languages will be taken into consideration");
              
           //     return;
          //  }
           List<string> languageCodes = new List<string>();
           
           if(args.Length != 0) {
          
	            string inputLC = args[0];
	            if(!LanguageManager.AvailableLanguages.ContainsValue(inputLC))
	            {   
					Console.WriteLine("Error: language code: "+inputLC+" is not supported by the game. Example language code for Portuguese (Brasilian): pt-BR");             
	                return;	            	
	                         
	            }
	            languageCodes.Add(inputLC);
            }else 
            {
           		languageCodes.AddRange(LanguageManager.AvailableLanguages.Values);
         
            }
            File.Delete(outputFile);
            StreamWriter writer = null;
            string oldLanguageCode = null;
           
            try{
            	writer = File.AppendText(outputFile);            
	            oldLanguageCode = LanguageManager.ActualLanguageCode;
	            
	            foreach(String languageCode in languageCodes)
	            {
	            
	            	
	            	List<String> missingKeys = new List<string>();
		            LanguageManager.SetLanguage(languageCode);   
	
	   				Console.WriteLine("Generating font ranges for language: " +languageCode);	            
		            
		            String[] keys = LanguageKey.GetAllLanguageKyes();
		            StringBuilder fullString = new StringBuilder();
		            foreach (string key in keys)
		            {
		              
		               if (String.Empty.Equals(LanguageResources.GetString(key)))
		               {
		               	  missingKeys.Add(key);
		                  // error
		               } else
		               { 
		               		fullString.Append(LanguageResources.GetString(key));
		               }
		               
		            }           
		            
		            // all possible chars            
		            char[] allchars = LanguageResources.BuildCharmap(fullString.ToString());
		            
		            
		            // get all ranges
		            string rangeString = "";
		            int lastValue = -1;
		            int rangeFrom = -1, rangeTo = -1;
		            bool firstRange = true;
		            var allcharsint = new List<int>();
		       
		            int i=0;
		            foreach(char c in allchars) {            	
		            	allcharsint.Add(Convert.ToInt32(c));
		            }
		            
		            if(addStandardRange) {		             
		            	
			            for(int k=33; k<=126; k++) {	
		            		if(!allcharsint.Contains(k)){
		            			allcharsint.Add(k);
		            		}
			            	
			            }
		            
		            }
		            allcharsint.Sort();
		          		            
		            bool lastRange = false;
		            foreach(int value in allcharsint) {								            	
		            	
		            	lastRange = (value== allcharsint[allcharsint.Count-1]);
		            	if(firstRange) {
		            		rangeFrom = rangeTo = lastValue = value;            	
		            		firstRange = false;
		            		continue;
		            	}
		            	
		            	if(lastValue + 1 == value) {
		            		// kontynuacja
		            		rangeTo = value;
		            	}else { 	            		
		            		// koniec range
		            		rangeString += rangeFrom + "-" + rangeTo+ " ";	            		
		            		rangeFrom = rangeTo = value;
		            		
		            	}
		            	if(lastRange) {
	            			rangeString += rangeFrom + "-" + rangeTo+ " ";	 
	            		}
		            	lastValue = value;
		            	
		            	
		           		
		            	
		            }	

		            
		            Console.WriteLine("Produced char range for language "+ languageCode+". Unique key codes:" + allcharsint.Count+ ", missing keys: "+missingKeys.Count+((missingKeys.Count > 0)? " - "+  String.Join("; ", missingKeys.ToArray()) : "" ));
		            writer.Write("\t/* "+languageCode+" - Unique key codes:" + allcharsint.Count+ ", missing key codes: "+missingKeys.Count +", generated at "+DateTime.Now.ToUniversalTime().ToString("u") +" */ "+ Environment.NewLine + "\tcode_points " + rangeString+Environment.NewLine+Environment.NewLine);
	            }
				Console.WriteLine("The output file is: "+ outputFile);
				
	            	
            }
            finally
            {
            	if(oldLanguageCode != null){
            		LanguageManager.SetLanguage(oldLanguageCode); 
            	}
            	if(writer != null){
            		writer.Close();            		
            	}
            }
            
           
            
            

           /* if (!File.Exists(filename))
            {
                MessageBox.Show("File '" + filename + "' does not exist");
                return;
            }*/
        //   languageCode

         
        }
        }
    }
}