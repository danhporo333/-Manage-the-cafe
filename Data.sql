CREATE DATABASE QuanLyQuanCafe
GO

USE QuanLyQuanCafe
GO

--đồ ăn
--bàn ghế
--Loại đồ ăn 
--tài khoản
--hóa đơn

create table TableFood
(
	id int 	identity primary key,
	name nvarchar(100),
	status nvarchar(100), --trống || có người
)

create table Account
(
	UserName nvarchar(100) PRIMARY KEY,
	Displayname nvarchar(100) NOT NULL,
	PassWord nvarchar(1000) DEFAULT 0,
	Type int NOT NULL -- 1: admin && 0: staff ,
)

create table FoodCategory
(
	id int 	identity primary key, 
	name nvarchar(100) NOT NULL
)

create table Food
(
	id int identity primary key,
	name nvarchar(100) NOT NULL,
	idCategory int NOT NULL,
	price float NOT NULL
	FOREIGN KEY (idCategory) REFERENCES dbo.FoodCategory(id)
)

create table Bill
(
	id int identity primary key,
	DateCheckIn Date NOT NULL,
	DateCheckOut Date,
	idTable int not null,
	discount int 
	status int not null default 0 --1 đã thanh toán && 0 là chưa thanh toán

	FOREIGN KEY (idTable) REFERENCES dbo.TableFood(id)

)

create table BillInfo
(
	id int identity primary key,
	idBill int not null,
	idFood int not null,
	count int not null  default 0

	FOREIGN KEY (idBill) REFERENCES dbo.Bill(id),
	FOREIGN KEY (idFood) REFERENCES dbo.Food(id)
)

DECLARE @i int = 0
while @i <=10
begin
	insert dbo.TableFood (name)VALUES (N'Bàn ' + CAST(@i AS nvarchar(100)))
	set @i = @i + 1
end
select *from TableFood
update TableFood set status = N'Trống'
update TableFood set status = N'Trống' where id = 6

select Food.name, BillInfo.count, Food.price, Food.price*BillInfo.count as totalPrice From BillInfo, Bill, Food where BillInfo.idBill = Bill.id and BillInfo.idFood = Food.id and bill.status = 0 and Bill.idTable = 4

select *from Food
select *from Bill
select *from BillInfo
select *from TableFood
update Bill set status = 1 where id = 1
update Bill set discount = 0;

select TableFood.name, Bill.totalPrice, DateCheckIn, DateCheckOut, discount 
from Bill, TableFood 
where DateCheckIn >= '20231030' and DateCheckOut <= '20231031' and Bill.status = 1 and TableFood.id = Bill.idTable