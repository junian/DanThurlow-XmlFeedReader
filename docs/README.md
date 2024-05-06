# XML FEED READER APP

We need an app to run on Windows that reads product data from an XML feed and creates product folders, text files and images on my desktop PC (see format below). 


## DIRECTORY STRUCTURE

```
Root folder
├──Products (folder)
|  ├──Product title 1 (folder)
|  |  ├──Price.txt
|  │  ├──A.jpg 
|  │  ├──B.jpg
|  │  └──etc.
|  └──etc.
└──LogFile.txt
```

## TITLE

The folder name is the product title. Any special characters that are not allowed in the folder name must be removed.


## PRICE

The text file name is the price. It must be a whole number with no decimal places. Eg. If the price is 27.99 then the text file will be named "27.txt". Note: If rounding is enabled in settings then "28.txt".


## DESCRIPTION

- The description is saved into the price text file.


## IMAGES

- Download images from the web using the hyperlinks in the XML feed and save to the hard drive.
- Products have 1 main image and up to 9 additional images. The main image is named "A.jpg" and additional images are named "B.jpg" "C.jpg" etc.


## ERROR HANDLING

The app must be designed with error handling so it:
- Never (Not responding)
- Never creates an incomplete product folder. If there is an error then it deletes the incomplete product folder and writes a new entry in the log file.


## DELETING OLD FOLDERS

Products folders that are not found in the XML file are deleted.


## REQUIRED FIELDS

The Title, Price, Description and Main image is required. Additional images are not required.


## ERROR LOG FILE

See example log file below.

```
24-04-2024 22:34 - finished with 822 products added, 25 updated, 7 deleted, 5 errors.
24-04-2024 22:31 - Copy failed for "Product C" no title was found.
24-04-2024 22:29 - Copy failed for "Product P" no price was found.
24-04-2024 22:25 - Copy failed for "Product Z" no description was found.
24-04-2024 22:24 - Copy failed for "Product Y" no main image was found.
24-04-2024 22:21 - Copy failed for "Product X" a product with same title already exists.
24-04-2024 22:20 - starting...
23-04-2024 20:46 - finished with 827 products added, 45 updates, 8 deleted, 0 errors.
23-04-2024 20:30 - starting...
22-04-2024 12:45 - finished with 827 products added, 256 updated, 12 deleted, 0 errors.
22-04-2024 12:30 - starting...
```

## SETTINGS

1. Feed URL:______
2. Output root folder: _______
3. Enable price rounding to the nearest whole dollar (tick box).
4. Add text to start of descriptions (user can append some plain text to the beginning of the descriptions).
5. Add text to end of descriptions (user can append some plain text to the end of the descriptions).
6. Save Settings button.
7. Get Products button - this starts the app and shows a text message: 
"Importing product information..."
"Import complete."


## TOKENS

For settings 4 and 5:
- User can enter plain text into a large multi-line text field.
- Users can include the name of any XML element inside the text as a token (eg. "<product_link>") and the app will replace the token with the element value.
- Instructions should be given if the user needs to include any escape characters.


## PERFORMANCE

To make the app run faster, it only needs to update the product information if the products "last_modified" date in the XML feed is newer than the date modified on the product folder. If no "last_modified" element is found then it must update.
