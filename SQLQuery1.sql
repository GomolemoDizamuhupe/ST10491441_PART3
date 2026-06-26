/*Creating db*/
Create database prog_task_1;

/*Using the db*/
USE [prog_task_1];

/*Creating the table*/
Create table tasks(
task_id int primary key identity(1,1),
task_name varchar(225),
task_description varchar(225),
task_dueDate varchar(20),
task_status varchar(20),
);

/*Selecting the table*/
Select * from tasks;
