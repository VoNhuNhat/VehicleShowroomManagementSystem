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

insert into UserAccount values('Demo','admin','123456','ha noi','admin@gmail.com','1234567890',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,1)
go