CREATE PROCEDURE [dbo].[TableDataPopulating]

AS

--------References to NULL--------

UPDATE dbo.[User]
SET DepartmentId = NULL

UPDATE dbo.[Department]
SET HeadId = NULL

UPDATE dbo.[Payment]
SET OrderId = NULL

--------Customer--------

DELETE FROM dbo.[Customer]

INSERT INTO dbo.[Customer]([Name],[Phone],[Email]) VALUES('Stewart C. Macias','(733) 361-7600','nunc.sed.pede@iaculisenimsit.com'),('Price Barton','(358) 630-8366','ultrices.mauris.ipsum@dolorFuscefeugiat.edu'),('Amena Barnes','(558) 699-1551','lorem.vehicula@mollislectus.org'),('Darius Lowe','(394) 401-8843','cursus.a.enim@Duismi.edu'),('Vivien Burke','(870) 827-8556','ac.mattis.semper@idante.co.uk'),('Larissa K. Landry','(786) 290-0920','et@sapienmolestie.co.uk'),('Wendy Bonner','(452) 185-8099','ante.Maecenas@lobortisrisusIn.co.uk'),('Aspen V. Jacobs','(679) 192-4676','mauris@fermentumrisusat.edu'),('Glenna Velasquez','(960) 334-3371','Quisque.fringilla.euismod@Aeneanegetmetus.com'),('Giselle Carr','(789) 318-8235','pede.nonummy@sagittis.net'),('Brianna Durham','(209) 192-2547','molestie@SuspendisseduiFusce.org'),('Emerson Wong','(267) 958-1438','scelerisque@auctorvelit.org'),('Alan K. Petersen','(164) 762-5766','arcu@liberoat.ca'),('Jena R. Delaney','(447) 904-1102','Integer.mollis.Integer@necligulaconsectetuer.net'),('Mallory V. Schultz','(770) 305-5682','risus@ornareelit.org'),('Colt E. Espinoza','(301) 359-5759','in.consectetuer@penatibus.edu'),('Eliana Best','(514) 533-8453','erat.vel@risusDonec.org'),('Kylynn Hogan','(735) 853-9464','in@Aeneaneget.net'),('Noble N. Bonner','(919) 730-1651','laoreet.lectus@luctus.ca'),('Rae Clements','(488) 473-8755','ornare.egestas@nisidictum.ca'),('Chastity S. Atkinson','(743) 779-9284','mollis.Phasellus@DuisgravidaPraesent.co.uk'),('Brent X. Juarez','(140) 768-6188','auctor.ullamcorper.nisl@faucibuslectusa.com'),('Nell H. Fowler','(650) 269-6685','fringilla@nunc.ca'),('Ivy Rich','(752) 720-8472','magna.Duis@ipsumdolor.co.uk'),('Jaime Strong','(640) 979-6742','sodales@lacus.org'),('Kasimir Y. Sharp','(148) 255-2798','Donec.felis.orci@CurabiturdictumPhasellus.ca'),('Addison G. Mueller','(513) 141-2745','velit.justo@neque.edu'),('Tiger K. Franks','(603) 230-5708','imperdiet.dictum@vestibulum.edu'),('Mariko Z. Boyer','(773) 679-8674','Nulla@morbi.ca'),('Quinn Mcneil','(573) 386-7731','cubilia.Curae.Phasellus@at.ca');

--------Department--------

DELETE FROM dbo.[Department]

INSERT INTO dbo.[Department]([Name],[Phone]) VALUES('Accounting','(693) 712-2396'),
('Advertising','(107) 559-7093'),
('Asset Management','(154) 309-7610'),
('Customer Relations','(138) 127-4828'),
('Customer Service','(447) 601-1281'),
('Finances','(608) 637-0250'),
('Human Resources','(148) 103-6705'),
('Legal Department','(962) 277-4017'),
('Media Relations','(596) 702-5180'),
('Public Relations','(236) 902-4973'),
('Quality Assurance','(533) 326-2697'),
('Sales and Marketing','(289) 655-1968'),
('Research and Development','(649) 149-8955'),
('Tech Support','(232) 493-7366'),
('Delivery Department','(746) 800-6316');

--------User--------

DELETE FROM dbo.[User]

INSERT INTO dbo.[User]([FullName],[Position],[Email],[Phone],[BirthDate],[Gender],[Login],[Password]) VALUES('Helena Makarchuk','President','helenamakarchuk@gmail.com','(899) 824-2411','06/22/1995','F','l1','p1'),('Drake P. Battle','Store Manager','rutrum.eu.ultrices@lobortis.co.uk','(533) 848-4256','10/04/1994','M','Isaac','PPL18KUM4WY'),('Garrison S. Collins','Manager','libero.nec@eu.org','(265) 291-1132','07/11/1986','M','Evelyn','UGM42PXH0TC'),('Gannon Bell','Regional Manager','turpis.Nulla@interdumSed.net','(286) 716-8170','05/13/1977','M','Branden','FKU88XAB2YQ'),('Griffith F. Delaney','President','montes.nascetur@malesuadafames.co.uk','(815) 362-1930','05/12/1989','M','Fay','OUR21VBI2KF'),('Elton Cantu','Delivery Driver','Integer@vestibulumMaurismagna.net','(458) 412-6920','04/29/2000','F','Philip','GLX90TNV1YX'),('Declan Travis','President','ipsum.primis.in@ipsumnonarcu.ca','(700) 823-2546','01/24/1991','M','Kareem','UYQ01KQB1RM'),('Quail M. Mcleod','Manager','placerat@netusetmalesuada.net','(480) 166-0543','09/16/1999','F','Ivory','UGB50GBN0IG'),('Walter M. Kim','President','accumsan@erat.ca','(766) 510-5903','04/15/1983','M','Camilla','LKW83YET6UR'),('Duncan Stuart','Regional Manager','ac@loremauctor.org','(863) 131-6294','06/20/1977','F','Caesar','LHZ49BIO2MJ'),('Karina C. May','Assistant','Donec.sollicitudin.adipiscing@lectusjusto.edu','(876) 920-7666','12/09/1986','M','Walker','BIF75JCM6WE'),('Kadeem Carter','Intern','dignissim.tempor.arcu@malesuada.com','(561) 225-1538','06/24/1978','F','Bell','PML36LWI3RO'),('Aaron Obrien','Assistant','Fusce@arcuiaculisenim.co.uk','(183) 917-6817','06/21/1978','F','Lilah','USD11MOO7TR'),('Sarah Mendez','Regional Manager','pede.et@Aliquamnec.ca','(847) 837-7715','11/27/1976','F','India','VXN02VXT5BR'),('Jenette Watts','Assistant','luctus.et@congueInscelerisque.com','(677) 402-1116','02/23/1977','M','Hashim','DYQ62WDW9CR'),('Aileen J. Vance','Assistant','enim@mifringilla.edu','(844) 951-7262','10/15/1981','F','Chaim','TBV71NQP7XT'),('Minerva W. Joseph','Intern','neque.non.quam@vestibulumMaurismagna.org','(146) 708-1419','09/04/1999','F','Kim','ZFL54MAU2LM'),('Quintessa R. Case','Assistant','nascetur.ridiculus.mus@arcuSed.edu','(172) 795-4685','11/05/1989','F','Kiara','RVR77SPL0QD'),('Britanney M. Neal','President','nec.enim.Nunc@loremsit.com','(346) 505-6837','10/05/1977','F','Colleen','ZPV90GIJ3MM'),('Genevieve T. Abbott','Delivery Driver','nec.tempus@temporeratneque.co.uk','(598) 943-3846','10/14/2000','F','Chiquita','UXN47DPX6BT'),('Mercedes Z. Flowers','Store Manager','in.faucibus.orci@intempus.edu','(149) 295-7545','12/02/1998','F','Oscar','EFZ24WQR8WC'),('Emmanuel Petty','Store Manager','commodo@vitaesodales.edu','(241) 383-8901','01/27/1981','F','Ingrid','HIT87ZKD5FF'),('Priscilla Kennedy','President','diam.Sed.diam@vulputatenisisem.co.uk','(405) 736-5294','06/02/1999','F','Daphne','AXB85FKL1PR'),('Demetria Q. Wright','Store Manager','magna.a@estmollis.com','(546) 943-5787','08/31/1988','M','Cassady','RPE20NSS8AA'),('Karyn Terrell','Regional Manager','Pellentesque@Suspendisse.co.uk','(907) 241-0788','10/06/1978','F','Orli','DMB80NQW9MU'),('Lewis W. Sparks','Team Leader','Donec@turpis.co.uk','(726) 983-2659','11/23/1981','M','Hayley','UFA42WUN5JG'),('Aspen P. Robertson','Store Manager','Phasellus.ornare.Fusce@lectus.edu','(763) 613-4517','05/06/1985','M','Cailin','MKM62JNN6KY');

--------Payment--------

DELETE FROM dbo.[Payment]
INSERT INTO dbo.[Payment]([Status],[Sum],[Method]) VALUES('1','6681','0'),('1','2736','0'),('0','1344','1'),('1','5222','0'),('0','4848','1'),('0','9119','1'),('1','8503','0'),('1','7118','0'),('1','0858','0'),('0','5749','1'),('1','9222','0'),('1','3758','0'),('1','5568','1'),('1','1362','0'),('0','2120','1'),('1','4923','1'),('0','9536','1'),('0','2587','0'),('0','1996','0'),('1','1059','1'),('1','0434','1'),('0','2739','0'),('0','5985','0'),('0','8625','1'),('0','7613','0'),('1','4224','1'),('1','7172','1'),('0','2667','1'),('0','6721','1'),('1','2273','0'),('1','9717','1'),('0','2269','1'),('0','4285','0'),('0','8369','1'),('1','7801','1'),('1','5177','0'),('1','7880','0'),('1','6648','1'),('0','4013','1'),('1','6041','1'),('0','7712','0'),('0','3037','0'),('1','2062','1'),('1','1225','0'),('1','5685','0'),('1','2780','0'),('0','5008','1'),('1','4306','0'),('0','1304','0'),('0','4046','1'),('1','0365','1'),('0','5611','1'),('1','7376','1'),('0','5849','0'),('1','4765','0'),('1','0784','0'),('1','4217','0'),('0','3044','0'),('1','4923','1'),('1','8732','1');

--------Order--------

DELETE FROM dbo.[Order]

RETURN 0
