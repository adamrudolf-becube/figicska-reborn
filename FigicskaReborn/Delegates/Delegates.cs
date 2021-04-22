using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FigicskaReborn
{
    delegate void DrawDelegate();
    delegate void PlayerGainedLife();
    delegate void PlayerLostLife();
    delegate PlayerControlEnumeration PlayerDecision();
    delegate void GameStepPlayers();

    delegate void LifePickedUp(Life life);
    delegate void PlayerDied(Player whoDied);
    delegate void PlayerMoved(int X, int Y);
    delegate void GameOverDelegate(Player winner);
    delegate void DeployedEvilThingDelegate(EvilThing deployedEvilThing);

    public delegate void ObjectOnFieldDrawDelegate(ObjectOnField o);
    public delegate void ObjectOnFieldClearDelegate(int x, int y);
}
