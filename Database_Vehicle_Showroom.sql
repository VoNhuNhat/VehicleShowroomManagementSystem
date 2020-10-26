create database Vehicle_Showroom_Management_System
go
use Vehicle_Showroom_Management_System
go

create table UserAccount (
	UserId int primary key identity,
	FullName varchar(256),
	UserName varchar(256),
	Password varchar(256),
	Address text,
	Email varchar(100),
	PhoneNumber varchar(50),
	CreatedDate Date,
	UpdatedDate Date,
	Status int 
)
go

select * from UserAccount
go
insert into UserAccount values('Administrator','admin','MTIzNDU2','Bach Khoa Aptech','c1808j1@gmail.com','1234567890',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,1)
go
/*
mk: 123456
*/


create proc Insert_UserAccount
	@FullName varchar(256),
	@UserName varchar(256),
	@Password varchar(256),
	@Address text,
	@Email varchar(100),
	@PhoneNumber varchar(50)
	as
	begin
	insert into UserAccount values(@FullName,@UserName,@Password,@Address,@Email,@PhoneNumber,CURRENT_TIMESTAMP,NULl,0)
	end
go

create proc Update_UserAccount
	@UserId int,
	@FullName varchar(256),
	@UserName varchar(256),
	@Password varchar(256),
	@Address text,
	@Email varchar(100),
	@PhoneNumber varchar(50)
	as
	begin
	update UserAccount set FullName = @FullName, UserName = @UserName, Password = @Password, Address = @Address,PhoneNumber = @PhoneNumber,UpdatedDate = CURRENT_TIMESTAMP where UserId = @UserId
	end
go

create proc Update_Password
	@UserId int,
	@Password varchar(256)
	as
	begin
	update UserAccount set Password = @Password,UpdatedDate = CURRENT_TIMESTAMP where UserId = @UserId
	end
go


create table Brands(
BrandId int primary key,
BrandName varchar(256),
image text
)
go

create table ModelCars(
ModelCarId int primary key identity,
ModelCarName varchar(256),
BrandId int references Brands(BrandId),
PriceOutput float
)
go

create table Images(
ImageId int primary key,
ModelCarId int references ModelCars(ModelCarId),
name text
)
go

create table PurchaseOrders(
PurchaseOrderId int primary key,
ModelCarId int references ModelCars(ModelCarId),
TotalPriceOutput float,
QuantityInput int,
PurchaseDate Date,
Status int
)
go

create table Cars(
ModerNumber varchar(100) primary key,
PurchaseOrderId int references PurchaseOrders(PurchaseOrderId),
CarName varchar(256),
PriceInput float,
SeatQuantity int,
Color varchar(50),
Gearbox varchar(256),
Engine varchar(256),
FuelConsumption float,
KilometerGone float,
Status int,
Checking int,
)
go

create table Customers(
CustomerId int primary key,
UserId int references UserAccount(UserId),
FullName varchar(256),
Address text,
Email varchar(256),
Phone varchar(256),
CreatedDate Date,
)
go


create table Orders(
OrderId int primary key,
ModerNumber varchar(100) references Cars(ModerNumber),	
CustomerId int references Customers(CustomerId), 
TotalMoney float,
CreatedDate Date,
UpdateDate Date,
Status int
)
go


