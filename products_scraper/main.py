from datetime import datetime
import os
import zipfile
from random import randint
import pymupdf
import csv
import shutil
import requests
from json import dump
import re

OUTPUT_DIR_BASEPATH = "output"
TEMP_DIR_BASEPATH = "temp"
os.makedirs(TEMP_DIR_BASEPATH, exist_ok=True)
os.makedirs(OUTPUT_DIR_BASEPATH, exist_ok=True)

PARTS_IMAGES_FILE = "https://cdn.rebrickable.com/media/downloads/ldraw/parts_0.zip"
PARTS_IMAGES_ZIP_PATH = f"{TEMP_DIR_BASEPATH}/parts.zip"
PARTS_IMAGES_PATH = f"{TEMP_DIR_BASEPATH}/parts_images"

# Getting the files info ==================
print("LEGO Instructions Scraper!")

print("\nProvide the following information ==================\n")

product_model = input("Enter the model number of the product [default: timestamp]: ")

if product_model == "":
    product_model = str(datetime.now().replace(microsecond=0)).replace(" ", "").replace(":", "").replace("-", "")

if (not product_model.isnumeric()):
    print("The product name must be numeric")
    exit()

product_model = int(product_model)

instructions_path = input("Enter the path to the instructions PDF:")
instructions_path = "/home/darguima/Downloads/TreviFountain_21020_zip/TreviFountain_21020_instructions.pdf"

parts_path = input("Enter the path to the parts CSV:")
parts_path = "/home/darguima/Downloads/TreviFountain_21020_zip/TreviFountain_21020_parts.csv"

output_dir = f"{OUTPUT_DIR_BASEPATH}/{product_model}"
os.makedirs(output_dir)

print("\n====================================================\n")
# =========================================

# Download the parts images file ==================
print("Downloading the parts images file ...")
if os.path.exists(PARTS_IMAGES_ZIP_PATH):
    print(f"Parts Images File already downloaded. Using cache file from `{PARTS_IMAGES_ZIP_PATH}`")
else:
    response = requests.get(PARTS_IMAGES_FILE, stream=True)
    
    if response.status_code == 200:
        with open(PARTS_IMAGES_ZIP_PATH, "wb") as file:
            for chunk in response.iter_content(chunk_size=1024):
                file.write(chunk) 
    else:
        print(f"Failed to download the file. Status code: {response.status_code}")
        exit()

with zipfile.ZipFile(PARTS_IMAGES_ZIP_PATH, 'r') as zip_ref:
    zip_ref.extractall(f"{TEMP_DIR_BASEPATH}/parts_images")

print(f"Parts images file available at {PARTS_IMAGES_PATH}/")
print("\n====================================================\n")
# =================================================

# In fault: model, name, price, description, product thumbnail
product = {
    "instructions": [],
    "parts": []
}

# Extracting the insctructions images ==================
print("Extracting the instructions images ...")
doc = pymupdf.open(instructions_path)
for page_num, page in enumerate(doc):
    instruction_img_name = f"page_{page_num + 1}.png"
    instruction_img_path = os.path.join(output_dir, instruction_img_name )

    pix = page.get_pixmap()
    pix.save(instruction_img_path)

    product["instructions"].append({
        "qnt_parts": randint(1, 5),
        "image": instruction_img_name
    })

print("\n====================================================\n")
# ======================================================

# Extracting the parts needed ==========================
print("Extracting the parts needed ...")
with open(parts_path, mode='r', newline='', encoding='utf-8') as file:
    csv_reader = csv.reader(file)

    next(csv_reader)
    
    for row in csv_reader:
        # Append a tuple of (first_column, ninth_column) to the list
        part_id = row[0]

        if part_id == "Total qty":
            break

        match = re.match(r'\d+', part_id)
        if match:
            part_id = int(match.group(0))
        else:
            print(f"Part id not valid: {part_id}")
            continue

        part_img_name = f"part_{part_id}.png"
        part_img_path = os.path.join(output_dir, part_img_name)

        if os.path.exists(f"{PARTS_IMAGES_PATH}/{part_id}.png"):
            shutil.copy(f"{PARTS_IMAGES_PATH}/{part_id}.png", f"{output_dir}/{part_img_name}")
        else:
            print(f"Part image not found for part_id: {part_id}")
            part_img_path = None

        product["parts"].append({
            "id": part_id,
            "qnt": int(row[8]),
            "img": part_img_name 
        })
        
print("\n====================================================\n")
# ======================================================

print("Generating the product.json file ...")

with open(f"{output_dir}/product.json", "w") as f:
    dump(product, f, indent=4)
