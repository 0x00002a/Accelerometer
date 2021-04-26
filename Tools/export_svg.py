import sys
import os
import subprocess as sp 
from multiprocessing import Pool, cpu_count

dims = 48

def export_to_png(file: str):
    parts = os.path.splitext(file)
    if parts[1] == ".svg":
        outname = parts[0] + ".png"
        print("{} -> {}".format(file, outname))
        sp.run(["D:/Program Files/Inkscape/bin/inkscape.exe", "--export-type=png", "-w", str(dims), "-h", str(dims), "--export-filename={}".format(outname), file], shell=True)
        
    
def export_mp(infiles: [str]):
    jobs = Pool(cpu_count())
    jobs.map(export_to_png, infiles)

if __name__ == '__main__':
    infile = "."
    if len(sys.argv) > 1:
        infile = sys.argv[1]
    
    infiles = []
    if os.path.isdir(infile):
        infiles = os.listdir(infile)
    elif len(sys.argv) > 1:
        infiles = sys.argv[1:]
    
    export_mp(infiles)
        


