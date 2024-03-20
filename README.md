An attendee from one of my AU classes asked if I knew of an existing program that could read data from an Excel file to populate custom properties in an Autocad file (DwgProps). I didn't, but had done all the pieces in one way or another, so I cobbled this together. Currently this reads from a CSV file, and can process multiple drawings (one drawing per row).

The CSV file contents:

![image](https://github.com/leadensky/CsvToDwgProps/assets/5571068/d74518e1-a123-475e-934d-7ec4b7148600)

The first drawing's DwgProps after the command is run:

![image](https://github.com/leadensky/CsvToDwgProps/assets/5571068/e78fc493-9220-461b-ad0c-4f2b4112181e)

The second drawing's DwgProps:

![image](https://github.com/leadensky/CsvToDwgProps/assets/5571068/d2a2366b-6943-47ba-9d6d-02c84fa7484c)
