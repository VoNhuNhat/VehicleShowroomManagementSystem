create database Vehicle_Showroom_Management_System
go
use Vehicle_Showroom_Management_System
go

create table Brands(
BrandId int primary key,
BrandName varchar(256),
image text
)

create table ModelCars(
ModelCarId int primary key identity,
ModelCarName varchar(256),
BrandId int references Brands(BrandId),
PriceOutput float
)

create table Images(
ImageId int primary key,
ModelCarId int references ModelCars(ModelCarId),
name text
)

create table PurchaseOrders(
PurchaseOrderId int primary key,
ModelCarId int references ModelCars(ModelCarId),
TotalPriceOutput float,
QuantityInput int,
PurchaseDate Date,
Status int
)

create table Cars(
ModerNumber int primary key,
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

create table Customers(
CustomerId int primary key,
UserId int references UserAccount(UserId),
FullName varchar(256),
Address text,
Email varchar(256),
Phone varchar(256),
CreatedDate Date,
)



create table Orders(
OrderId int primary key,
ModerNumber int references Cars(ModerNumber),	
UserId int references UserAccount(UserId),
CustomerId int references Customers(CustomerId), 
TotalMoney float,
CreatedDate Date,
UpdateDate Date,
Status int

)