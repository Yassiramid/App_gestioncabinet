create database ProjetFinEtude
use ProjetFinEtude
create table visite(numV int primary key identity(1,1),Examen nvarchar(200),Conclusion  nvarchar(200),traitement  nvarchar(200),motif  nvarchar(20),montant float,payé bit)
create table patient(numP nvarchar(20)primary key,nomP nvarchar(50),prénomP nvarchar(50),sexe  nvarchar(10),age int,dateCreation date,télèphone nvarchar(10),situationF nvarchar(20),mutuelle nvarchar(20),adresse nvarchar(50))
create table RDV(numR int primary key identity(1,1),dateR date,heure nvarchar(5),motifR  nvarchar(20),visite int foreign key references visite(numV),patient nvarchar(20) foreign key references patient(numP)) 
create table Antécedant(numA int primary key identity(1,1),nomA nvarchar(20),dateD date,traiter nvarchar(3),Observation nvarchar(200),patient nvarchar(20) foreign key references patient(numP)) 
create table FichierMédical(numF int primary key identity(1,1),typeF nvarchar(20),imageF image,visite int foreign key references visite(numV),Observation nvarchar(200))
create table payement(numP int primary key identity(1,1),visite int foreign key references visite(numV))
create table Users(
id int identity(1,1) primary key,
userName nvarchar (100) unique not null,
password nvarchar (100) not null,
firstName nvarchar(100) not null,
lastName nvarchar(100) not null,
position nvarchar (100) not null,
email nvarchar(100) unique not null,
profilePicture varbinary(max)
)

create proc addUser
	@userName nvarchar(100),
	@password nvarchar(100),
	@firstName nvarchar(100),
	@lastName nvarchar(100),
	@position nvarchar(100),
	@email nvarchar(100),
	@profile varbinary(max)
	as
	insert into Users 
	values (@userName,@password, @firstName, @lastName,@position,@email,@profile)
go

create proc editUser
	@userName nvarchar(100),
	@password nvarchar(100),
	@firstName nvarchar(100),
	@lastName nvarchar(100),
	@position nvarchar(100),
	@email nvarchar(100),
	@profile varbinary(max),
	@id int
	as
	update  Users	
	set userName=@userName,password=@password,firstName=@firstName,lastName= @lastName,position= @position,email=@email, profilePicture=@profile  
	where id=@id 
go

create proc removeUser
	@id int
	as
	delete from Users where id=@id 
go

create proc loginUser
	@user nvarchar (100),
	@password nvarchar (100)
	as
	select *from Users 
	where (userName=@user and password=@password ) or (email=@user and password=@password)
go

create proc selectAllUsers
	as
	select *from Users 
go

create proc selectUser
	@findValue nvarchar (100)
	as
	select *from Users 
	where userName= @findValue or firstName like @findValue+'%' or email=@findValue
go


exec addUser 'admin','admin','Jackson','Collins','Administrator','Support@SystemAll.biz',null
exec addUser 'Ben','abc123456','Benjamin','Thompson','Accounting','BenThompson@MyCompany.com',null 
exec addUser 'Kathy','abc123456','Kathrine','Smith','Receptionist','KathySmith@MyCompany.com',null


exec selectAllUsers
exec loginUser 'admin', 'admin'


go