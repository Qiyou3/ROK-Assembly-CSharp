// Decompiled with JetBrains decompiler
// Type: CodeHatch.Blocks.Commands.BlockCommandHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Blocks.Inventory;
using CodeHatch.Blocks.Networking.Events;
using CodeHatch.Common;
using CodeHatch.Core;
using CodeHatch.Engine.Characters;
using CodeHatch.Engine.Core.Commands;
using CodeHatch.Engine.Networking;
using CodeHatch.Inventory.Blueprints;
using CodeHatch.Networking.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;

#nullable disable
namespace CodeHatch.Blocks.Commands
{
  public class BlockCommandHandler : MonoBehaviour
  {
    public void Awake() => CommandManager.RegisterCommands((object) this);

    public void OnDestroy()
    {
    }

    [SubCommand("/build structures (radius) (material)", "Creates a bunch of random structures.")]
    [SubCommand("/build structure (radius) (material)", "Creates a structure.")]
    [SubCommand("/build pyramid (radius) (material)", "Creates a pyramid out of blocks.")]
    [SubCommand("/build spiralr (radius) (material)", "Creates a reverse spiral out of blocks.")]
    [Command("/build [sphere|ball|cube|box|maze|plane|cylinder|structure|structures|pyramid] (radius) (material)", "Creates a shape out of blocks.", "codehatch.blocks.build", new string[] {"shape"})]
    [SubCommand("/build cylinder (radius) (material)", "Creates a cylinder out of blocks.")]
    [SubCommand("/build plane (radius) (material)", "Creates a plane out of blocks.")]
    [SubCommand("/build maze (radius) (material)", "Creates a maze out of blocks. Odd radius makes walls around the maze.")]
    [SubCommand("/build box (radius) (material)", "Creates a box out of blocks.")]
    [SubCommand("/build cube (radius) (material)", "Creates a cube out of blocks.")]
    [SubCommand("/build ball (radius) (material)", "Creates a ball out of blocks.")]
    [SubCommand("/build sphere (radius) (material)", "Creates a sphere out of blocks.")]
    [SubCommand("/build spiral (radius) (material)", "Creates a spiral out of blocks.")]
    public void Build(CommandInfo cmd)
    {
      Player player = cmd.Player;
      Character currentCharacter = player.CurrentCharacter;
      string[] args = cmd.Args;
      if (args.Length >= 1)
      {
        RootCubeGrid allCubeGrid = BlockManager.AllCubeGrids[0];
        int result1 = 3;
        if (args.Length >= 2)
          int.TryParse(args[1], out result1);
        Vector3 vector3 = Vector3.zero;
        if (currentCharacter != null && (UnityEngine.Object) currentCharacter.Entity != (UnityEngine.Object) null)
        {
          vector3 = currentCharacter.Entity.Position;
          Vector3 forward = currentCharacter.Entity.GetOrCreate<LookBridge>().Forward;
          RaycastHit hitInfo;
          if (Physics.Raycast(vector3, forward, out hitInfo, 1000f))
            vector3 = hitInfo.point;
        }
        Vector3Int localCoordinate = allCubeGrid.WorldToLocalCoordinate(vector3);
        byte result2 = 1;
        if (args.Length == 3)
          byte.TryParse(args[2], out result2);
        if (result2 < (byte) 0)
        {
          if (args.Length == 3)
          {
            IEnumerable<TilesetBlueprint> source = ((IEnumerable<InvItemBlueprint>) InvBlueprints.Instance.AllDefinedBlueprints).Where<InvItemBlueprint>((Func<InvItemBlueprint, bool>) (b => b.Has<TilesetBlueprint>())).Select<InvItemBlueprint, TilesetBlueprint>((Func<InvItemBlueprint, TilesetBlueprint>) (b => b.Get<TilesetBlueprint>()));
            string[] matches = source.Select<TilesetBlueprint, string>((Func<TilesetBlueprint, string>) (t => t.name)).ToArray<string>().Match(args[2]);
            if (matches.Length > 1)
            {
              player.SendError("Please specify a single material. '{0}' matches {1} materials ({2}).", (object) args[2], (object) matches.Length, (object) ((IEnumerable<string>) matches).JoinToString<string>(","));
              return;
            }
            if (matches.Length == 0)
            {
              player.SendError("Cannot find material by '{0}'.", (object) args[2]);
              return;
            }
            result2 = source.First<TilesetBlueprint>((Func<TilesetBlueprint, bool>) (t => t.name == matches[0])).MaterialID;
          }
        }
        try
        {
          string lower = args[0].ToLower();
          if (lower != null)
          {
            // ISSUE: reference to a compiler-generated field
            if (BlockCommandHandler.\u003C\u003Ef__switch\u0024map1 == null)
            {
              // ISSUE: reference to a compiler-generated field
              BlockCommandHandler.\u003C\u003Ef__switch\u0024map1 = new Dictionary<string, int>(13)
              {
                {
                  "sphere",
                  0
                },
                {
                  "ball",
                  1
                },
                {
                  "cube",
                  2
                },
                {
                  "box",
                  3
                },
                {
                  "maze",
                  4
                },
                {
                  "plane",
                  5
                },
                {
                  "cylinder",
                  6
                },
                {
                  "helix",
                  7
                },
                {
                  "spiral",
                  8
                },
                {
                  "spiralr",
                  9
                },
                {
                  "pyramid",
                  10
                },
                {
                  "structure",
                  11
                },
                {
                  "structures",
                  12
                }
              };
            }
            int num;
            // ISSUE: reference to a compiler-generated field
            if (BlockCommandHandler.\u003C\u003Ef__switch\u0024map1.TryGetValue(lower, out num))
            {
              switch (num)
              {
                case 0:
                  Coroutiner.StartStaticCoroutine(this.CreateSphereOvertime(cmd, result2, result1, localCoordinate));
                  return;
                case 1:
                  Coroutiner.StartStaticCoroutine(this.CreateBallOvertime(cmd, result2, result1, localCoordinate));
                  return;
                case 2:
                  Coroutiner.StartStaticCoroutine(this.CreateCubeOvertime(cmd, result2, result1, localCoordinate));
                  return;
                case 3:
                  Coroutiner.StartStaticCoroutine(this.CreateBoxOvertime(cmd, result2, result1, localCoordinate));
                  return;
                case 4:
                  Coroutiner.StartStaticCoroutine(this.CreateMazeOvertime(cmd, result2, result1, localCoordinate));
                  return;
                case 5:
                  Coroutiner.StartStaticCoroutine(this.CreatePlaneOvertime(cmd, result2, result1, localCoordinate));
                  return;
                case 6:
                  Coroutiner.StartStaticCoroutine(this.CreateCylinder(cmd, result2, result1, localCoordinate, UnityEngine.Random.Range(10, 100)));
                  return;
                case 7:
                  Coroutiner.StartStaticCoroutine(this.CreateHelix(cmd, result2, result1, localCoordinate));
                  return;
                case 8:
                  Coroutiner.StartStaticCoroutine(this.CreateSpiral(cmd, result2, result1, localCoordinate, true));
                  return;
                case 9:
                  Coroutiner.StartStaticCoroutine(this.CreateSpiral(cmd, result2, result1, localCoordinate, false));
                  return;
                case 10:
                  Coroutiner.StartStaticCoroutine(this.CreatePyramid(cmd, result2, result1, localCoordinate));
                  return;
                case 11:
                  Coroutiner.StartStaticCoroutine(this.CreateStructure(cmd, result2, result1, localCoordinate));
                  return;
                case 12:
                  Coroutiner.StartStaticCoroutine(this.CreateStructures(cmd, result2, result1, localCoordinate));
                  return;
              }
            }
          }
          cmd.Player.SendError("You cannot build a " + args[0] + ". Only a sphere, cube or maze.");
        }
        catch (Exception ex)
        {
          this.LogException<BlockCommandHandler>(ex);
        }
      }
      else
      {
        player.SendMessage("Shapes: Sphere, Ball, Cube, Maze");
        List<TilesetBlueprint> list = ((IEnumerable<InvItemBlueprint>) InvBlueprints.Instance.AllDefinedBlueprints).Where<InvItemBlueprint>((Func<InvItemBlueprint, bool>) (b => b.Has<TilesetBlueprint>())).Select<InvItemBlueprint, TilesetBlueprint>((Func<InvItemBlueprint, TilesetBlueprint>) (b => b.Get<TilesetBlueprint>())).ToList<TilesetBlueprint>();
        StringBuilder stringBuilder = new StringBuilder();
        foreach (TilesetBlueprint tilesetBlueprint in list)
          stringBuilder.AppendFormat("{0} = {1}, ", (object) tilesetBlueprint.name, (object) tilesetBlueprint.MaterialID);
        if (list.Count > 0)
          player.SendMessage("Materials: {0}", (object) stringBuilder.ToString().Trim(' ', ','));
        else
          player.SendMessage("There are no materials.");
      }
    }

    private int BlocksPerFrame => (int) Math.Ceiling(1.0 / (double) Time.deltaTime);

    [DebuggerHidden]
    private IEnumerator CreateHelix(
      CommandInfo cmd,
      byte material,
      int radius,
      Vector3Int origin,
      int height = -1)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new BlockCommandHandler.\u003CCreateHelix\u003Ec__Iterator6()
      {
        radius = radius,
        cmd = cmd,
        material = material,
        origin = origin,
        height = height,
        \u003C\u0024\u003Eradius = radius,
        \u003C\u0024\u003Ecmd = cmd,
        \u003C\u0024\u003Ematerial = material,
        \u003C\u0024\u003Eorigin = origin,
        \u003C\u0024\u003Eheight = height,
        \u003C\u003Ef__this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CreateStructures(
      CommandInfo cmd,
      byte material,
      int radius,
      Vector3Int origin)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new BlockCommandHandler.\u003CCreateStructures\u003Ec__Iterator7()
      {
        radius = radius,
        origin = origin,
        cmd = cmd,
        material = material,
        \u003C\u0024\u003Eradius = radius,
        \u003C\u0024\u003Eorigin = origin,
        \u003C\u0024\u003Ecmd = cmd,
        \u003C\u0024\u003Ematerial = material,
        \u003C\u003Ef__this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CreateStructure(
      CommandInfo cmd,
      byte material,
      int radius,
      Vector3Int origin)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new BlockCommandHandler.\u003CCreateStructure\u003Ec__Iterator8()
      {
        cmd = cmd,
        material = material,
        radius = radius,
        origin = origin,
        \u003C\u0024\u003Ecmd = cmd,
        \u003C\u0024\u003Ematerial = material,
        \u003C\u0024\u003Eradius = radius,
        \u003C\u0024\u003Eorigin = origin,
        \u003C\u003Ef__this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CreatePyramid(
      CommandInfo cmd,
      byte material,
      int radius,
      Vector3Int origin)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new BlockCommandHandler.\u003CCreatePyramid\u003Ec__Iterator9()
      {
        origin = origin,
        radius = radius,
        material = material,
        cmd = cmd,
        \u003C\u0024\u003Eorigin = origin,
        \u003C\u0024\u003Eradius = radius,
        \u003C\u0024\u003Ematerial = material,
        \u003C\u0024\u003Ecmd = cmd,
        \u003C\u003Ef__this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CreateRegularSpiral(
      CommandInfo cmd,
      byte material,
      int radius,
      Vector3Int origin,
      bool direction,
      int height = -1)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new BlockCommandHandler.\u003CCreateRegularSpiral\u003Ec__IteratorA()
      {
        height = height,
        radius = radius,
        origin = origin,
        direction = direction,
        material = material,
        \u003C\u0024\u003Eheight = height,
        \u003C\u0024\u003Eradius = radius,
        \u003C\u0024\u003Eorigin = origin,
        \u003C\u0024\u003Edirection = direction,
        \u003C\u0024\u003Ematerial = material,
        \u003C\u003Ef__this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CreateSpiral(
      CommandInfo cmd,
      byte material,
      int radius,
      Vector3Int origin,
      bool direction,
      int height = -1)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new BlockCommandHandler.\u003CCreateSpiral\u003Ec__IteratorB()
      {
        origin = origin,
        radius = radius,
        direction = direction,
        material = material,
        height = height,
        \u003C\u0024\u003Eorigin = origin,
        \u003C\u0024\u003Eradius = radius,
        \u003C\u0024\u003Edirection = direction,
        \u003C\u0024\u003Ematerial = material,
        \u003C\u0024\u003Eheight = height,
        \u003C\u003Ef__this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CreateCylinder(
      CommandInfo cmd,
      byte material,
      int radius,
      Vector3Int origin,
      int height = 10)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new BlockCommandHandler.\u003CCreateCylinder\u003Ec__IteratorC()
      {
        origin = origin,
        height = height,
        radius = radius,
        material = material,
        cmd = cmd,
        \u003C\u0024\u003Eorigin = origin,
        \u003C\u0024\u003Eheight = height,
        \u003C\u0024\u003Eradius = radius,
        \u003C\u0024\u003Ematerial = material,
        \u003C\u0024\u003Ecmd = cmd,
        \u003C\u003Ef__this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CreateSphereOvertime(
      CommandInfo cmd,
      byte material,
      int radius,
      Vector3Int origin)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new BlockCommandHandler.\u003CCreateSphereOvertime\u003Ec__IteratorD()
      {
        radius = radius,
        origin = origin,
        material = material,
        cmd = cmd,
        \u003C\u0024\u003Eradius = radius,
        \u003C\u0024\u003Eorigin = origin,
        \u003C\u0024\u003Ematerial = material,
        \u003C\u0024\u003Ecmd = cmd,
        \u003C\u003Ef__this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CreateCubeOvertime(
      CommandInfo cmd,
      byte material,
      int radius,
      Vector3Int origin)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new BlockCommandHandler.\u003CCreateCubeOvertime\u003Ec__IteratorE()
      {
        radius = radius,
        origin = origin,
        material = material,
        cmd = cmd,
        \u003C\u0024\u003Eradius = radius,
        \u003C\u0024\u003Eorigin = origin,
        \u003C\u0024\u003Ematerial = material,
        \u003C\u0024\u003Ecmd = cmd,
        \u003C\u003Ef__this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CreateBoxOvertime(
      CommandInfo cmd,
      byte material,
      int radius,
      Vector3Int origin)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new BlockCommandHandler.\u003CCreateBoxOvertime\u003Ec__IteratorF()
      {
        radius = radius,
        origin = origin,
        material = material,
        cmd = cmd,
        \u003C\u0024\u003Eradius = radius,
        \u003C\u0024\u003Eorigin = origin,
        \u003C\u0024\u003Ematerial = material,
        \u003C\u0024\u003Ecmd = cmd,
        \u003C\u003Ef__this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CreateBallOvertime(
      CommandInfo cmd,
      byte material,
      int radius,
      Vector3Int origin)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new BlockCommandHandler.\u003CCreateBallOvertime\u003Ec__Iterator10()
      {
        radius = radius,
        origin = origin,
        material = material,
        cmd = cmd,
        \u003C\u0024\u003Eradius = radius,
        \u003C\u0024\u003Eorigin = origin,
        \u003C\u0024\u003Ematerial = material,
        \u003C\u0024\u003Ecmd = cmd,
        \u003C\u003Ef__this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CreatePlaneOvertime(
      CommandInfo cmd,
      byte material,
      int radius,
      Vector3Int origin)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new BlockCommandHandler.\u003CCreatePlaneOvertime\u003Ec__Iterator11()
      {
        radius = radius,
        origin = origin,
        material = material,
        cmd = cmd,
        \u003C\u0024\u003Eradius = radius,
        \u003C\u0024\u003Eorigin = origin,
        \u003C\u0024\u003Ematerial = material,
        \u003C\u0024\u003Ecmd = cmd,
        \u003C\u003Ef__this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CreateMazeOvertime(
      CommandInfo cmd,
      byte material,
      int radius,
      Vector3Int origin)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new BlockCommandHandler.\u003CCreateMazeOvertime\u003Ec__Iterator12()
      {
        radius = radius,
        cmd = cmd,
        origin = origin,
        material = material,
        \u003C\u0024\u003Eradius = radius,
        \u003C\u0024\u003Ecmd = cmd,
        \u003C\u0024\u003Eorigin = origin,
        \u003C\u0024\u003Ematerial = material,
        \u003C\u003Ef__this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CalculateMaze(bool[,] grid, Vector2Int pos, int count = -1)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new BlockCommandHandler.\u003CCalculateMaze\u003Ec__Iterator13()
      {
        count = count,
        pos = pos,
        grid = grid,
        \u003C\u0024\u003Ecount = count,
        \u003C\u0024\u003Epos = pos,
        \u003C\u0024\u003Egrid = grid,
        \u003C\u003Ef__this = this
      };
    }

    [Command("/instantbuild", "Toggles instant building for the local player", "codehatch.blocks.debug", "ib")]
    public void InstantBuild(CommandInfo cmd)
    {
      InstantBuildCommandEvent theEvent;
      if (cmd.SenderIsServer)
      {
        InstantBuildCommandEvent buildCommandEvent = new InstantBuildCommandEvent();
        buildCommandEvent.Recipients = new List<Player>()
        {
          cmd.Player
        };
        theEvent = buildCommandEvent;
      }
      else
      {
        InstantBuildCommandEvent buildCommandEvent = new InstantBuildCommandEvent();
        buildCommandEvent.Recipients = new List<Player>()
        {
          cmd.Player
        };
        buildCommandEvent.SkipServerHandle = true;
        theEvent = buildCommandEvent;
      }
      EventManager.CallEvent((BaseEvent) theEvent);
    }
  }
}
