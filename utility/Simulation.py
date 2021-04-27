import math as m
import time
from .ConsolePrint1 import ConsolePrint

galaxy = []
"""
import galaxy_generation as gg
galaxy = gg.generate_galaxy()
"""





#===== TESTS =========
galax = [["yolo", "pirate", ["20", "2", []] , 30, "none", (10,10)]]
MF = [["51", "0", (0,0), "pirate"]]







#====== VECTORS MATHS =========

def LookAt(a , b): #gives vector from a looking to b normalized
    x = b[0] - a[0]
    y = b[1] - a[1]
    length = m.sqrt((x*x) + (x*y))
    return ( x/length, y/length)

def Distance(a, b):#distance between a and b
    x = b[0] - a[0]
    y = b[1] - a[1]
    return m.sqrt((x*x) + (y*y))







# ======= S I M U L A T I O N =========================

def EnterSystem(fleet, system, galaxy):#simulate battle when a fleet enter a system, return new owner ad system fleet as battleResult
    system = galaxy[system]
    fleet_power = int(fleet[0])
    system_fleet_power = system[3]
    if fleet[3] == system[1]:
        return(fleet[3], system_fleet_power + fleet_power)
    system_station_power = int(system[2][0])
    system_power = system_fleet_power + system_station_power
    if fleet_power > system_power:
        owner = fleet[3]
        system_fleet = fleet_power - system_power
    else:
        owner = system[1]
        system_fleet = system_power- fleet_power
        if system_fleet<0:
            system_fleet = 0
    return (owner, system_fleet)


def UpdateSystem_Battle(battleResult, system, galaxy):#update system after battle, takes new owner and system fleet as battleResult and system number
    galaxy[system][1] = battleResult[0]
    galaxy[system][3] = battleResult[1]
    return galaxy



def update_fleets(galaxy, moving_fleets, deltaTime): #takes a galaxy, a list of moving fleets and a time / returns updated galaxy and update fleets
    for i in range(len(moving_fleets)):
        fleet = moving_fleets[i]
        destination = galaxy[int(fleet[1])][5]
        vector = LookAt(fleet[2], destination)
        new_position_vector = ( vector[0]*deltaTime, vector[1]*deltaTime)
        new_position = (fleet[2][0]+new_position_vector[0], fleet[2][1]+new_position_vector[1])
        if Distance(new_position, destination) <= deltaTime:
            battleResult = EnterSystem(fleet, int(fleet[1]), galaxy)
            galaxy = UpdateSystem_Battle(battleResult, int(fleet[1]), galaxy)
            moving_fleets.pop(i)
        else:
            moving_fleets[i][2] = (new_position)
    return galaxy, moving_fleets


def UpdateSimulation(galaxy, moving_fleets, playerList, deltaTime):

    galaxy, moving_fleets = update_fleets(galaxy, moving_fleets, deltaTime)
    playerList = UpdatePlayers(PlayerList, deltaTime)
    return galaxy, moving_fleets, playerList



def UpdatePlayers(PlayerList, deltaTime):
    l = []
    for player in PlayerList:
        sub = []
        sub.append(player[0])
        sub.append(str( float(player[2]) + (float(player[3]) * deltaTime)    ))
        sub.append(player[3])
        l.append(sub)
    return l




"""
    def ConsolePrint(a,b):
        print(a)

a = UpdatePlayers([["yolo", "#7deb34", "3", "2.5"], ["hehe", "#7deb34", "1.25", "0.2"]], 1)
print(a)
"""





#
