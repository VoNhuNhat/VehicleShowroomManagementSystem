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

insert into UserAccount values('Administrator','admin','MTIzNDU2','Bach Khoa Aptech','c1808j1@gmail.com','1234567890',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,1)
go
--select * from UserAccount
--go
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
--select * from Brands
--go

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
Id int primary key identity,
PurchaseOrderId varchar(256) unique,
ModelCarId int references ModelCars(ModelCarId),
QuantityCarImport int,
OrderDate Date,
CreatedDate DateTime,
UpdatedDate DateTime,
Status int
)
go

create proc Insert_PurchaseOrder
@PurchaseOrderId varchar(256),
@ModelCarId int,
@QuantityCarImport int,
@OrderDate Date
as
begin
insert into PurchaseOrders values(@PurchaseOrderId,@ModelCarId,@QuantityCarImport,@OrderDate,CURRENT_TIMESTAMP,NULL,0)
end
go

--insert into PurchaseOrders values('SC0000003',1,100,CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,NULL,0)
--go

--delete from PurchaseOrders
--go

--select * from PurchaseOrders where OrderDate >= '11/26/2020'
--go

create table Cars(
CarId int primary key identity,
ModelNumberCar varchar(100),
Id int references PurchaseOrders(Id),
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
Sold int
)
go


create proc Insert_Car
	@ModelNumberCar varchar(100),
	@Id int,
	@CarName varchar(256),
	@PriceInput float,
	@PriceOutput float,
	@SeatQuantity int,
	@Color varchar(50),
	@Gearbox varchar(256),
	@Engine varchar(256),
	@FuelConsumption float,
	@KilometerGone float,	
	@Status int,
	@Checking int,
	@PurchaseOrderDate Date
as
begin
	insert into Cars values(@ModelNumberCar, @Id, @CarName, @PriceInput, @PriceOutput, @SeatQuantity, @Color, @Gearbox, @Engine, @FuelConsumption, @KilometerGone, @Status, @Checking, @PurchaseOrderDate, CURRENT_TIMESTAMP, NULL,0)
end
go

create proc Update_Car_Sold
	@ModelNumberCar varchar(100),
	@Sold int,
	@Checking int
as
begin
	update Cars set Sold = @Sold, Checking = @Checking where ModelNumberCar = @ModelNumberCar
end
go

	--select * from Cars where Id = 3
	--go
	--delete from Cars
	--go
	

create table Images(
ImageId int primary key identity,
CarId int references Cars(CarId),
Name varchar(MAX),
Status int
)
go

--select * from Images
--go
--delete from Images
--go


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
--select * from Customers
--go
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
Id int primary key identity,
OrderId varchar(256) unique,
ModelNumberCar varchar(100),	
CustomerId int references Customers(CustomerId), 
TotalMoney float,
OrderDate Date,
CreatedDate DateTime,
UpdateDate DateTime,
Status int
)
go

--select * from Orders
--go

create proc Insert_Order
	@OrderId varchar(256),
	@ModelNumberCar varchar(100),
	@CustomerId int,
	@TotalMoney float
as
begin
	insert into Orders values(@OrderId,@ModelNumberCar,@CustomerId,@TotalMoney,CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,NULL,0)
end
go

create proc Update_Order_Paid
	@Id int
as
begin
	update Orders set Status = 1 where Id = @Id
end
go