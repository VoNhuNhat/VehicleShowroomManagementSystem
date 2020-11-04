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
	Birthday Date not null,
	CreatedDate DateTime,
	UpdatedDate DateTime,
	Status int 
)
go

select * from ModelCars where ModelCarId !=  10 and ModelCarName = 'Mazda 5'
go

delete from ModelCars where ModelCarId = 9
go
insert into UserAccount values('Administrator','admin','MTIzNDU2','Bach Khoa Aptech','c1808j1@gmail.com','1234567890',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,1)
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
	@PhoneNumber varchar(50),
	@Birthday Date
	as
	begin
	insert into UserAccount values(@FullName,@UserName,@Password,@Address,@Email,@PhoneNumber,@Birthday,CURRENT_TIMESTAMP,NULl,0)
	end
go

create proc Update_UserAccount
	@UserId int,
	@FullName varchar(256),
	@UserName varchar(256),
	@Password varchar(256),
	@Address text,
	@Email varchar(100),
	@PhoneNumber varchar(50),
	@Birthday Date
	as
	begin
	update UserAccount set FullName = @FullName, UserName = @UserName, Password = @Password,Email = @Email, Address = @Address,PhoneNumber = @PhoneNumber,Birthday = @Birthday,UpdatedDate = CURRENT_TIMESTAMP where UserId = @UserId
	end
go

create table Brands(
BrandId int primary key identity,
BrandName varchar(256),
Image text,
CreatedDate DateTime,
UpdatedDate DateTime
)
go

create proc Insert_Brand
	@BrandName varchar(256),
	@image text 
	as
	begin
	insert into Brands values(@BrandName,@image,CURRENT_TIMESTAMP,NULL)
	end
go

create proc Update_Brand
@BrandId int,
@BrandName varchar(256),
@image text
as 
begin 
update Brands set BrandName=@BrandName, image = @image, UpdatedDate = CURRENT_TIMESTAMP where BrandId= @BrandId
end 
go 

create table ModelCars(
ModelCarId int primary key identity,
ModelCarName varchar(256),
BrandId int references Brands(BrandId),
CreatedDate DateTime,
UpdatedDate DateTime
)
go

create proc Insert_ModeCar
@ModelCarName varchar(256),
@BrandId int
as
begin
	insert into ModelCars values(@ModelCarName,@BrandId,CURRENT_TIMESTAMP,NULL)
end
go


create table PurchaseOrders(
PurchaseOrderId int primary key identity,
ModelCarId int references ModelCars(ModelCarId),
QuantityCarImport int,
OrderDate Date,
CreatedDate DateTime,
UpdatedDate DateTime,
Status int
)
go

create table Cars(
ModelNumberCar varchar(100) primary key,
PurchaseOrderId int references PurchaseOrders(PurchaseOrderId),
CarName varchar(256),
PriceInput float,
PriceOutput float,
SeatQuantity int,
Color varchar(50),
Gearbox varchar(256),
Engine varchar(256),
FuelConsumption float,
KilometerGone float,
Status int,
Checking int,
PurchaseOrderDate Date,
CreatedDate DateTime,
UpdatedDate DateTime,
)
go

create table Images(
ImageId int primary key identity,
ModelNumberCar varchar(100) references Cars(ModelNumberCar),
Name text	
)
go



create table Customers(
CustomerId int primary key identity,
UserId int references UserAccount(UserId),
FullName varchar(256),
Address text,
Email varchar(256),
Phone varchar(256),
Birthday Date,
CreatedDate DateTime,
UpdatedDate DateTime,
)
go

create proc Insert_Customer
	@UserId int,
	@FullName varchar(256),
	@Address text,
	@Email varchar(256),
	@Phone varchar(256),
	@Birthday Date
as
begin
	insert into Customers values(@UserId, @FullName, @Address, @Email, @Phone, @Birthday, CURRENT_TIMESTAMP, NULl)
end
go

create proc Update_Customer
	@CustomerId int,
	@FullName varchar(256),
	@Address text,
	@Email varchar(256),
	@Phone varchar(256),
	@Birthday Date
as
begin
	update Customers set FullName = @FullName, Email = @Email, Address = @Address, Phone = @Phone, Birthday = @Birthday, UpdatedDate = CURRENT_TIMESTAMP where CustomerId = @CustomerId
end
go

create table Orders(
OrderId int primary key identity,
ModelNumberCar varchar(100),	
CustomerId int references Customers(CustomerId), 
TotalMoney float,
CreatedDate DateTime,
UpdateDate DateTime,
Status int
)
go

