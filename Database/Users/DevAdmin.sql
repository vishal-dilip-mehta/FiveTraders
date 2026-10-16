CREATE USER DevAdmin
WITH PASSWORD = 'ExtraVeganza!@!#';
       
ALTER ROLE db_owner ADD MEMBER DevAdmin;

