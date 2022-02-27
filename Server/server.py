#
# Filename: server.py
# Project:  Assignment 3
# By: Mustafa Efiloglu
# Date: February 26, 2021
# Description:  server python
#

import asyncio
import os
import socket
import threading
from datetime import datetime
from time import sleep

import aiofiles as aiof
import yaml

class ClntThrd(threading.Thread):

    def __init__(self, clntsckt):
        threading.Thread.__init__(self)
        self.csocket = clntsckt

    def run(self):
        msg_data = self.csocket.recv(MSG_DLMT)
        message = msg_data.decode()

        msgel = message.split(MSG_DLMT)

        if len(msgel) == CLNT_MSGEL \
                and msgel[IDX_CLNT].isnumeric():
            if clntId.get(int(msgel[IDX_CLNT])) is None:
                clntId[int(msgel[IDX_CLNT])] = 0
            else:
                clntId[int(msgel[IDX_CLNT])] += 1

            if clntId[int(msgel[IDX_CLNT])] < MSG_LMT:
                prcs_msg(msgel)


class MngClnt:
    def __init__(self):
        thread = threading.Thread(target=self.run)
        thread.daemon = True
        thread.start()

    @staticmethod
    def run():
        while True:
            clntId.clear()
            sleep(CLR_INTV)


def prcs_msg(msgel):
    if msgel[IDX_TM].isnumeric() and msgel[IDX_LGLVL].isnumeric():

        if 0 <= int(msgel[IDX_LGLVL]) < len(LG_LVL):
            msgel[IDX_LGLVL] = LG_LVL.get((int(msgel[IDX_LGLVL])))

            if int(msgel[IDX_CLNT]) in clntOf \
                    and msgel[IDX_LGLVL] == LG_LVL.get(LG_LVLON):
                clntOf.remove(int(msgel[IDX_CLNT]))

            if int(msgel[IDX_CLNT]) not in clntOf:
                lp = asyncio.new_event_loop()
                asyncio.set_event_loop(lp)

                try:
                    lp.run_until_complete(write_to_file(LG_DRCT + LG_NMA, msgel))
                finally:
                    lp.run_until_complete(lp.shutdown_asyncgens())
                    lp.close()

                if msgel[IDX_LGLVL] == LG_LVL.get(LG_LVLOF):
                    clntOf.append(int(msgel[IDX_CLNT]))


async def write_to_file(filename, msgel):
    current_time = datetime.utcfromtimestamp(float(msgel[IDX_TM])
                                             / MLS_SCD).strftime(TM_FRMT)[:-3]

    async with aiof.open(filename, "a") as out:
        await out.write(LG_FRMT.format(time=current_time,
                                          id=msgel[IDX_CLNT],
                                          log_level=msgel[IDX_LGLVL],
                                          message=msgel[IDX_MSG]))
        await out.flush()


def ld_cnf():
    global LCLHST
    global PT
    global RCV_BFR
    global MSG_DLMT
    global MLS_SCD
    global LG_NMA
    global LG_DRCT
    global TM_FRMT
    global LG_FRMT
    global LG_LVL
    global LG_LVLOF
    global LG_LVLON
    global MSG_LMT
    global CLR_INTV
    global IDX_TM
    global IDX_CLNT
    global IDX_LGLVL
    global IDX_MSG

    dt_yaml = yaml.safe_load(open(CFG_YAML))

    sv_sttng = dt_yaml["server"]
    LCLHST = sv_sttng.get("ip")
    PT = sv_sttng.get("pt")
    RCV_BFR = sv_sttng.get("maximum_msg")
    MSG_DLMT = sv_sttng.get("msg_seperator")
    MLS_SCD = sv_sttng.get("mil_sec")

    file_settings = dt_yaml["logging"]
    LG_NMA = file_settings.get("file")
    TM_FRMT = file_settings.get("date")
    LG_FRMT = file_settings.get("logg")
    LG_LVL = file_settings.get("logging_lvl")
    LG_DRCT = file_settings.get("file_dir")
    LG_LVLOF = file_settings.get("logging_lvlOff")
    LG_LVLON = file_settings.get("logging_lvlOn")
    IDX_TM = file_settings.get("date_idx")
    IDX_CLNT = file_settings.get("clnt_i_idx")
    IDX_LGLVL = file_settings.get("logg_lvl_idx")
    IDX_MSG = file_settings.get("msg_idx")
    noise_settings = dt_yaml["nse_hdlg"]
    MSG_LMT = noise_settings.get("mx_nmbr_msg")
    CLR_INTV = noise_settings.get("date_lmt")


def main():
    ld_cnf()

    if not os.path.exists(LG_DRCT):
        os.makedirs(LG_DRCT)

    if not os.path.exists(LG_DRCT + LG_NMA):
        open(LG_DRCT + LG_NMA, "w").close()

    sv = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sv.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    sv.bind((LCLHST, PT))

    MngClnt()

    print("Server started on", LCLHST, ":", PT)
    print("Waiting for client(s)...")

    while True:
        sv.listen(1)
        clientsock, client_address = sv.accept()
        newthread = ClntThrd(clientsock)
        newthread.start()

CFG_YAML = "config.yaml"
LCLHST = ""
PT = 0 
RCV_BFR = 0
LG_LVL = {}
TM_FRMT = ""
MLS_SCD = 0
CLR_INTV = 0
MSG_LMT = 0
MSG_DLMT = ""
LG_NMA = ""
LG_DRCT = ""
LG_FRMT = ""
LG_LVLOF = 0
LG_LVLON = 0
IDX_TM = 0
IDX_CLNT = 0
IDX_LGLVL = 0
IDX_MSG = 0
CLNT_MSGEL = 4
clntId = {}
clntOf = []

if __name__ == "__main__":
    main()
