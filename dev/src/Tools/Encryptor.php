<?php
error_reporting(E_ALL);
ini_set("display_errors","1");

$hash = "127;253;227;220;63;109;102;74;169;250;128;88;197;125;156;118;130;22;159;228;";

class WofLicense 
{
	const C_ENHANCED_VERSION_LICENSE = 'asZc2czA4l6a12sd';
	const C_ENHANCED_FILE = "enhanced.dat";

	const C_KEY = "1c!Haa6bq0LC*G3FeG2oONCdFwSP4ev[";
	const C_IV = "$04Bxi0NxEbIkvkLSMgh2LP58Dwc*WCH";



	protected static function addpadding($string, $blocksize = 32)
	{
		$len = strlen($string);
		$pad = $blocksize - ($len % $blocksize);
		$string .= str_repeat(chr($pad), $pad);
		return $string;
	}

	protected static function strippadding($string)
	{
		$slast = ord(substr($string, -1));
		$slastc = chr($slast);
		$pcheck = substr($string, -$slast);
		if(preg_match("/$slastc{".$slast."}/", $string)){
			$string = substr($string, 0, strlen($string)-$slast);
			return $string;
		} else {
			return false;
		}
	}

	protected function encrypt($string = "", $key, $iv)
	{
		return base64_encode(mcrypt_encrypt(MCRYPT_RIJNDAEL_256, $key, WofLicense::addpadding($string), MCRYPT_MODE_CBC, $iv));
	}

	protected function decrypt($string = "", $key, $iv)
	{	
		$string = base64_decode($string);
		return WofLicense::strippadding(mcrypt_decrypt(MCRYPT_RIJNDAEL_256, $key, $string, MCRYPT_MODE_CBC, $iv));
	}

		
	public function buildLicenseFile($hash)
	{
			
		$hashEncoded = $this->encrypt($hash, WofLicense::C_KEY, WofLicense::C_IV);
		//echo $hashEncoded."\n";
		
		$licenseKey = substr($hashEncoded, 0, 32);

		$licenseEncoded = $this->encrypt(WofLicense::C_ENHANCED_VERSION_LICENSE, $licenseKey, WofLicense::C_IV);
		//echo $licenseEncoded."\n";
		
		$file = fopen (WofLicense::C_ENHANCED_FILE, "w");
		fwrite($file, "\r\n".$hashEncoded."\r\n".$licenseEncoded);
		fclose ($file);  

	}

}

$wl = new WofLicense();

$wl->buildLicenseFile($hash);


//echo decrypt($encoded, $key, $iv);

?>