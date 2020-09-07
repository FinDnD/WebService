using Espresso401_WebService.Data;
using Espresso401_WebService.Models.Interfaces;
using Espresso401_WebService.Models.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;

namespace Espresso401_WebServiceTests
{
    public class DatabaseTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        protected readonly AppDbContext _appDb;
        protected readonly UserDbContext _userDb;

        protected readonly IDungeonMaster _dungeonMaster;
        protected readonly IParty _party;
        protected readonly IPlayer _player;
        protected readonly IRequest _request;

        public DatabaseTest()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            _appDb = new AppDbContext(
                new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options);
            _userDb = new UserDbContext(
                new DbContextOptionsBuilder<UserDbContext>()
                .UseSqlite(_connection)
                .Options);

            _appDb.Database.EnsureCreated();
            _userDb.Database.EnsureCreated();

            _dungeonMaster = new DungeonMasterRepository(_appDb, _party, _request);
            _party = new PartyRepository(_appDb);
            _player = new PlayerRepository(_appDb, _party, _request);
            _request = new RequestRepository(_appDb, _party);
        }

        public void Dispose()
        {
            _appDb?.Dispose();
            _userDb?.Dispose();
            _connection?.Close();
        }
    }
}