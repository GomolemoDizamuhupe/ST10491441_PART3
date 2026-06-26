Create database prog_task_1;

USE [prog_task_1];

Create table tasks(
task_id int primary key identity(1,1),
task_name varchar(225),
task_description varchar(225),
task_dueDate varchar(20),
task_status varchar(20),
);

Select * from tasks;