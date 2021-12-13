using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Context;
using textadventure_backend_entitymanager.Models.Entities;
using textadventure_backend_entitymanager.Models.Responses;

namespace textadventure_backend_entitymanager.Services
{
    public class EnemyService
    {

        public EnemyService()
        {
            
        }

        public Enemy GenerateEnemy(int experience)
        {
             return new Enemy(experience);
        }
    }
}
