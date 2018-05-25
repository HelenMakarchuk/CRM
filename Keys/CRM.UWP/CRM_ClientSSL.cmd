makecert.exe ^
-n "CN=%1,O=TrustCompany,OU=Dev,L=Saint Petersburg,S=Saint Petersburg,C=RU" ^
-iv CRM_CARoot.pvk ^
-ic CRM_CARoot.cer ^
-pe ^
-a sha512 ^
-len 4096 ^
-b 01/01/2018 ^
-e 01/01/2040 ^
-sky exchange ^
-eku 1.3.6.1.5.5.7.3.2 ^
-sv %1.pvk ^
%1.cer

pvk2pfx.exe ^
-pvk %1.pvk ^
-spc %1.cer ^
-pfx %1.pfx ^
-po 123456