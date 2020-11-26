use  Aprenant;
CREATE TABLE Apprenant (
    id int  IDENTITY(1,1),
	Nom varchar(10),
	Prenom varchar(10),
	Email varchar(30),
	Telephone varchar(14),
	Age int,
	birthday date,
	country varchar(15),
	City varchar(15),
	Choice varchar(6)
);

ALTER TABLE Apprenant DROP COLUMN id 
ALTER TABLE Apprenant ADD id INT IDENTITY(1,1)


select * from Apprenant;

Insert into Apprenant(Nom,Prenom,Email,Telephone,Age,birthday,country, City,Choice)
values('Yassir', 'Hanzife', 'yassir@gmail.com','+212 623444344', '24','1996-10-26', 'Morocco','Safi', 'C#');
delete from Apprenant where id =1

update Apprenant set
Nom = 'yasser'
where id = 5;