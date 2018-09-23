import discord
import Client
import shlex
import argparse
import response
import threading
import analizemedicale
import json
from discord.ext import commands

token = "NDkwNTI3MjQ0MjY4MDc3MDU2.Dn6m3A.90szlTFeuNU7dqK-GCnOj8wRuts"

bot = commands.Bot(command_prefix="~")


@bot.event
async def on_ready():
    print("Bot is running and ready to use!")

@bot.command(pass_context = True)
async def analize(ctx):

    if(client.get_service_status() == False):
        await bot.say("Service unavailable")
        return

    entirecommand = ctx.message.content
    arguments = entirecommand.replace('~analize ', '')
    splinted = shlex.split(arguments)
    parser = argparse.ArgumentParser()
    parser.add_argument("-age")
    parser.add_argument("-sex")
    parser.add_argument("-on_thyroxine")
    parser.add_argument("-query_on_thyroxin")
    parser.add_argument("-on_antythyroid_medication")
    parser.add_argument("-thyroid_surgery")
    parser.add_argument("-query_hypothyroid")
    parser.add_argument("-query_hyperthyroid")
    parser.add_argument("-pregnant")
    parser.add_argument("-sick")
    parser.add_argument("-tumor")
    parser.add_argument("-lithium")
    parser.add_argument("-goitre")
    parser.add_argument("-TSH_measured")
    parser.add_argument("-TSH")
    parser.add_argument("-T3_measured")
    parser.add_argument("-T3")
    parser.add_argument("-TT4_measured")
    parser.add_argument("-TT4")
    parser.add_argument("-FTI_measured")
    parser.add_argument("-FTI")
    parser.add_argument("-TBG_measured")
    parser.add_argument("-TBG")
    args = parser.parse_args(splinted)

    persdata = analizemedicale.personaldata(sex=args.sex, age=args.age, on_thyroxine=args.on_thyroxine, query_on_thyroxin=args.query_on_thyroxin, on_antythyroid_medication=args.on_antythyroid_medication, thyroid_surgery=args.thyroid_surgery, query_hypothyroid=args.query_hypothyroid, query_hyperthyroid=args.query_hyperthyroid, pregnant=args.pregnant, sick=args.sick, tumor=args.tumor, lithium=args.lithium, goitre=args.goitre, TSH_measured=args.TSH_measured, TSH=args.TSH, T3_measured=args.T3_measured, T3=args.T3, TT4_measured=args.TT4_measured, TT4=args.TT4, FTI_measured=args.FTI_measured, FTI=args.FTI, TBG_measured=args.TBG_measured, TBG=args.TBG)
    jsonstring = json.dumps(persdata.__dict__)
    print(jsonstring)

    # sending data
    response = await client.SendData(jsonstring)
    await bot.say('Medical response: ' + str(response.get_chanse()) + ' ' + str(response.get_rest()))


def on_server_connected(sender, eargs):
    print("Connected to the medical service")

def on_server_connection_lost(sender, eargs):
    print("Connection to the medical service lost.. retying to connect..")
    newthread = threading.Thread(target=sender.DoConnectionUntilConnected())
    newthread.daemon = True
    newthread.start()


print("Trying to connect the bot")


client = Client.TcpClient('127.0.0.1', 5554)
client.on_connected += on_server_connected
client.on_connection_lost += on_server_connection_lost
newthread = threading.Thread(target = client.DoConnectionUntilConnected())
newthread.daemon = True
newthread.start()

bot.run(token)





