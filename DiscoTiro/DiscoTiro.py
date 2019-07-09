import discord
import Client
import shlex
import argparse
import response
import threading
import analizemedicale
import json
import requests
import hashlib
import imghdr
import asyncio
from multiprocessing import Process
from discord.ext import commands


with open("auth.json") as json_file:
	config = json.load(json_file)
	token = config["bot_token"]
	webhook = config["bot_webhook_url"]


bot = commands.Bot(command_prefix="~")


@bot.event
async def on_ready():
    print("Bot is running and ready to use!")

@bot.command(pass_context = True)
async def analize(ctx):

    if(client.get_service_status() == False):
        await ctx.send("Service unavailable")
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
    await ctx.send('Sansa de hypotiroida: ' + str(response.get_rest()))



@bot.command(pass_context = True)
async def pneumonia(ctx):
    errcode = await send_and_receive_response('pneumonia', ctx)

@bot.command(pass_context = True)
async def tuberculoza(ctx):
    errcode = await send_and_receive_response('tuberculoza', ctx)

@bot.command(pass_context = True)
async def bleeding(ctx):
    errcode = await send_and_receive_response('hemoragie', ctx)

@bot.command(pass_context = True)
async def breast(ctx):
    errcode = await send_and_receive_response('cancersan', ctx)
    
@bot.command(pass_context = True)
async def leukemia(ctx):
    errcode = await send_and_receive_response('leucemie', ctx)

@bot.command(pass_context = True)
async def malaria(ctx):
    errcode = await send_and_receive_response('malarie', ctx)




async def send_and_receive_response(disease, ctx):
    if(client.get_service_status() == False):
        await ctx.send("Service unavailable")
        return 2
        
    try:
        photo_url = ctx.message.attachments[0].url
        photo_content = requests.get(photo_url).content
        file_name_hash = hashlib.md5(photo_content)
    
    
        f = open('photos\\' + file_name_hash.hexdigest(),'wb')
        f.write(photo_content)
        f.close()
        
        
        if(imghdr.what('photos\\' + file_name_hash.hexdigest()) != None):
            response = await client.SendPhoto('pneumonia', 'photos\\' + file_name_hash.hexdigest())
            await ctx.send('pneumonia result: ' + str(response.get_rest()))
            return 0
            
        else:
            await ctx.send('Invalid photo')
            return 1
    
    except:
        await ctx.send('No photo has been sent')
        return -1


def on_server_connected(sender, eargs):
    print("Connected to the medical service")

def on_server_connection_lost(sender, eargs):
    print("Connection to the medical service lost.. retying to connect..")
    newthread = threading.Thread(target=sender.DoConnectionUntilConnected)
    newthread.daemon = True
    newthread.start()


print("Trying to connect the bot")


client = Client.TcpClient('127.0.0.1', 5554)
client.on_connected += on_server_connected
client.on_connection_lost += on_server_connection_lost
p1 = threading.Thread(target=client.DoConnectionUntilConnected)
p1.start()

bot.run(token)





