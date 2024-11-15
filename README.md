# Esoteric Picture Extractor

This .NET 8 solution leverages [magic bytes](https://en.wikipedia.org/wiki/List_of_file_signatures) to decompose large, concatenated binary files into individual components.

The following formats are currently supported:
* BZ2
* EMF
* GZIP
* JFIF (JPG)
* JP2
* PNG
* WMF
* HPI

## To Use

The command-line application must be executed with three parameters:

1. Source file path (e.g., `C:\foo.dat`)
2. Destination folder path (e.g, `C:\exportContents`)
3. Mode (e.g., comma separated values of `WMF`, `EMF`, `GZIP`, `HPI`, `JFIF`, `PNG`, `BZ2`, or `JP2`)

A sample data file named `combinedFiles.dat` is located within the `testFiles` folder. It is made up by concatenating numerous files together, which are also available for review reference. Running the application against the sample data file will decompose it back into individual files.

### Technology Stack

The solution is written with:
* .NET 8
* Magick.Net (only for merging HPI layers together, see below)

### Stream Process Pipeline

All files are processed with the following workflow:
1. Read bytes in sequential order
2. Isolate individual files based on magic bytes
3. *(Optional)* Perform additional transformations on each individual file
4. Write file to disk

## Background

Before Internet access became ubiquitous, people could only access a small selection of clip-art when creating documents and presentations. Options were typically limited to what came included with their installed office productivity software. However, it was possible to buy clip-art collections on floppy disks and CDs like the one shown below.

![12,000 Clip Art by COSMI cd case cover](./img/cosmi-clipart-cdcase.jpg)

Given that computers had limited computing power and storage space during this time period, these clip-art libraries consisted of small-sized vector images in [WMF and EMF formats](https://en.wikipedia.org/wiki/Windows_Metafile). Users could just copy these files directly from the disk onto their local computer for later use.

As time went on, some clip-art collections required users to install proprietary software to browse, access, and use the clip-art. Frequently written with Windows 3.1 in mind, this proprietary software will have compatibility problems with modern systems. In addition, these clip-art collections started including graphics in common formats like BMP, JPG, PNG or esoteric formats like HPI. 

### Proprietary Software

`12,000 Clip Art` by Cosmi requires users to install proprietary software to access clip-art. Users can only export images for use one-at-a-time.

![Proprietary software running on Windows XP to browse a clip-art library](./img/cosmi-photo-browser.jpg)

The software installs several large (100MB+) files. These files are named like `Vector0_CNT.dat` and `Raster0_CNT.dat`, suggesting that they contain clip-art data. This was confirmed by viewing the files in a binary viewer, showing that each file was the combination of hundreds of smaller files.

### What is HPI?

HPI is a proprietary image format created by Hemera Technologies and used extensively within `Photo Clip Art 10,000` by IdeaSoft. As a proprietary format, users had to install software to browse and export HPI-formatted images for use in other applications one-at-a-time.

Rather than installing such software, I opened up an HPI file in a binary viewer to discover that the format is just a container for one JPG file and one PNG file. The JPG file contains RGB data whereas the PNG file is a grayscale image used as an alpha channel. Not only does HPI combine the strengths of both formats (JPG compression, PNG transparency), it locks the image in a proprietary format that most users would not be able to bypass.

![JPG layer of an HPI-encoded file of a handled wicker basket](./img/HPI-jpg-layer.jpg)
![PNG alpha transparency layer of an HPI-encoded file of a handled wicker basket](./img/HPI-png-layer.png)

Since this project uses magic bytes to decompose files, HPI files behave interestingly:

* Running in `HPI` mode will combine the JPG/PNG files together into a transparency-enabled PNG with Magick.Net
* Running in `JPG`mode will export the RGB layer of an HPI file as a JPG
* Running in `PNG` mode will export the alpha layer of an HPI file as a PNG

### GZIP and BZ2 Are Not Image Formats

True, `GZIP` and `BZ2` are general-purpose compression algorithms. `12,000 Clip Art` by Cosmi leverages these two formats to compress `WMF` files to a smaller size.

Since `GZIP` lacks an end-of-file marker, this project has two known issues with its implementation:

* `GZIP` files are bigger than expected but can still successfully extract contents, and
* One junk/unreadable file will likely be created.

### Why This Software Exists

It is *expensive* to license photos from modern sites like [Pond5.com](https://www.pond5.com). Alternatively, the dated clip-art and photos in these collections are *royalty-free* and thus can be used with minimal restrictions for personal and professional projects. 

You don't need a modern photograph of a *entrega de equipaje* to create a flashcard for studying Spanish words! 

![Young family at an airport baggage claim conveyor belt](./img/young-family-at-baggage-claim.jpg)

Thus, this project was created to bulk convert clip-art from proprietary formats into formats compatible with modern computers. It was also a great opportunity to try streaming and processing data one byte at a time.

## License

The source code contained in this repository is [licensed under CC0](./LICENSE). However, this license *does not apply to any media files*.

* Stock photo(s) from `Art Explosion 500,000` by Nova Development under the terms of their end-user license agreement. 
* Stock photo(s) from `Photo Clip Art 10,000` by IdeaSoft under their royalty-free license. ©2004 IdeaSoft and its licensors.
* Stock photo(s) from `12,000 Clip Art #1` by Cosmi. ©2005 Cosmi Corporation and its licensors. All rights reserved.