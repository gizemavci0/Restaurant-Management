﻿
Create table tblMain
(
MainID int primary key identity,
aDate date,
aTime Varchar(15),
TableName varchar(10),
WaiterName varchar(15),
status varchar(15),
orderType varchar(15),
total float,
received float,
change float
)

Create table tblDetails
(
DetailID int primary key identity,
MainID int,
proID int,
qty int,
price float,
amount float,
)
