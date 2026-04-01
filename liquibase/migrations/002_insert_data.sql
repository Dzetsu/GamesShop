--liquibase formatted sql

--changeset you:002

insert into order_games.games (nameofgame, qty) values ('Counter-Strike', 1800);
insert into order_games.games (nameofgame, qty) values ('Minecraft', 2000);
insert into order_games.games (nameofgame, qty) values ('Valorant', 5);