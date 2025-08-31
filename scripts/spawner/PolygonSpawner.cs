using Godot;
using Godot.Collections;
using System;

namespace BossRush2;

/// <summary>
/// Utilises a generation pool, to spawn polygons randomly
/// </summary>
[GlobalClass]
public partial class PolygonSpawner : EntitySpawner
{
    /// The selection of polygon templates to be spawned
    [Export]
    protected Array<PolygonTemplate> SpawnPool;
    [Export]
    protected int PolygonCount = 0;

    Array<float> cachedPool = [];
    float totalWeight = 0f;

    /// <summary>
    /// Call after any changes are made to the PolygonSpawnPool
    /// </summary>
    public void CachePool()
    {
        totalWeight = 0f;
        cachedPool = [];

        foreach (var thisTemplate in SpawnPool)
        {
            totalWeight += thisTemplate.Weight;
            cachedPool.Add(totalWeight);
        }
    }

    /// <summary>
    /// Spawns a specific polygon by template
    /// </summary>
    public void SpawnSpecific(PolygonTemplate thisTemplate)
    {
        if (thisTemplate.SceneOverride == null)
        {
            Entity polygonToAdd = new()
            {
                _teams = {{"Side","Polygon"}},
                angularVelocity = ExtraMath.RandRange(-1.5f, 1.5f),
                stats = thisTemplate.stats
            };

            //Setting position
            Vector2 spawnSize = World.worldSize;
            float radius = thisTemplate.Collider.Radius;

            spawnSize -= new Vector2(radius, radius);
            polygonToAdd.Position = ExtraMath.RandVector(-spawnSize, spawnSize);

            //Setting sprites
            Sprite2D sprite = new()
            {
                Texture = thisTemplate.Sprite,
            };
            polygonToAdd.AddChild(sprite);

            //Setting physics hitboxes
            CollisionShape2D collider = new()
            {
                Shape = thisTemplate.Collider
            };

            polygonToAdd.AddChild(collider);

            CollisionShape2D collider2 = new()
            {
                Shape = thisTemplate.Collider
            };

            Hitbox hitbox = new()
            {
                owner = polygonToAdd,
                CollisionLayer = 0,
                CollisionMask = 0,
                antiCram = true
            };
            hitbox.AddChild(collider2);

            polygonToAdd.AddChild(hitbox);
            Spawn(polygonToAdd, 3);
        }
        else
        {
            throw new NotImplementedException("No custom scene support yet");
        }

    }

    /// <summary>
    /// Spawns a single polygon randomly
    /// </summary>
    public void SpawnSingle()
    {
        float selector = GD.Randf() * totalWeight;

        for (int i = 0; i < cachedPool.Count; i++)
        {
            if (selector < cachedPool[i])
            {
                SpawnSpecific(SpawnPool[i]);
                return;
            }
        }
    }

    public override void EnterGame()
    {
        CachePool();
        for (int i = 0; i < PolygonCount; i++)
        {
            SpawnSingle();
        }
    }
}
