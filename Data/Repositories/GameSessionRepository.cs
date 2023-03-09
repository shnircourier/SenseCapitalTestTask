using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Data.Repositories;

public class GameSessionRepository : IRepository<GameSession>
{
    private readonly DatabaseContext _context;

    public GameSessionRepository(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<List<GameSession>> Get()
    {
        return await _context.GameSessions.Where(g => g.IsGameEnded == false).ToListAsync();
    }

    public async Task<GameSession> Get(int id)
    {
        return await _context.GameSessions.FirstOrDefaultAsync(g => g.Id == id && g.IsGameEnded == false);
    }

    public async Task<GameSession> Create(GameSession entity)
    {
        var gameSession = await _context.GameSessions.AddAsync(entity);

        await Save();

        return gameSession.Entity;
    }

    public async Task<GameSession> Update(GameSession entity)
    {
        var gameSession =  _context.GameSessions.Update(entity);

        await Save();

        return gameSession.Entity;
    }

    public async Task<GameSession> Delete(int id)
    {
        var gameSession = await Get(id);

        if (gameSession is null)
        {
            throw new Exception("Игровая сессия не найдена");
        }

        var removedSession = _context.GameSessions.Remove(gameSession);

        await Save();

        return removedSession.Entity;
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}