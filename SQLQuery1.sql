create table Product(
 ProductId int not null identity(1,1) primary key,
 Name varchar(500) null,
 Price varchar(500) null,
 ImageURL varchar(500) null,
 Link varchar(500) null,
 Firm varchar(500) null,
 LastUpdated datetime null,
);
