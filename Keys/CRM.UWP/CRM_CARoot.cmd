makecert.exe ^
-n "CN=CRM_CARoot,OU=Dev,O=TrustCompany,L=Saint Petersburg,S=Saint Petersburg,C=RU" ^
-r ^
-pe ^
-a sha512 ^
-len 4096 ^
-cy authority ^
-sv CRM_CARoot.pvk ^
CRM_CARoot.cer

pvk2pfx.exe ^
-pvk CRM_CARoot.pvk ^
-spc CRM_CARoot.cer ^
-pfx CRM_CARoot.pfx ^
-po 123456