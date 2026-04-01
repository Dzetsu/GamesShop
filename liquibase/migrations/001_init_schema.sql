--liquibase formatted sql

--changeset you:001

create schema order_games; 

create table order_games.games
(
    id serial primary key,
    nameofgame text not null,
    qty bigint not null 
);

CREATE TABLE order_games.order (
    id BIGSERIAL PRIMARY KEY,
    username TEXT,
    datetime TEXT,
    nameofgame TEXT,
    status TEXT DEFAULT 'no paid',
    idempotent_key VARCHAR(255)
);

create table order_games.outbox(
    order_id serial primary key,
    status text
);