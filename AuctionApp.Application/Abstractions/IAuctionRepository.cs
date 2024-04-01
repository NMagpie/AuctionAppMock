﻿using AuctionApp.Domain.Models;

namespace AuctionApp.Application.Abstractions;
public interface IAuctionRepository
{
    Auction Create(Auction auction);

    Auction? GetById(int id);

    ICollection<Auction> GetAll();

    int GetLastId();
}
