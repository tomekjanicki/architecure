USE master
IF EXISTS(select * from sys.databases where name='{0}')
DROP DATABASE [{0}]

CREATE DATABASE [{0}]