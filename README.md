# Relinker
Tool to batch update path of lnk files.
____

## Why to use this tool?
My usual work flow at job implies to use many link files pointing to my folders at my OneDrive. 
That works for me but my organization is prone to change its name and when that happens they change 
the root name of our OneDrive folder breaking my links.

I developed this tool to batch change my link files to modify their target path to new root name. It 
works for me, but please read the LICENSE, if this tool breaks your computer, cause havoc at your home, 
or a thermonuclear happening in your neighborhood don't put your lawyers after me, just fill an 
[issue](https://github.com/dante-signal31/relinker/issues) at this page and I'll try to fix next version. 
Any help or [pull request](https://github.com/dante-signal31/relinker/pulls) is really welcome!

## Installation
Ninja way is cloning this repository, assessing code and building by your own, so you can check everything 
is right.

If you don't feel like that, go to release section and download latest one. Given enough time I may build a 
proper installer, but so far it is just a zip with all files bundled. Just decompress it in a folder you like 
and run executable from a console set in that folder.

## Usage
Just type exe command with no arguments and you'll get a handy usage help.

You need to set at least these parameters:
* **root**: Base folder to search links from. Relinker will find any link in a folder under that root.
* **original**: String to be substituted in target paths. If not found nothing is done. It is a basic substitution. 
Just surround your string between quotes.
* **modified**: String to modify target paths. String set with *original* will be replaced with this string.

There are some optional parameters you should be aware of:
* **verbose**: Print details during execution. Very useful to store that output in a text file to check for any 
error afterwards (just look for "ERROR" string).
* **simulate**: Perform search but do not change link files. Normally used with *verbose* flag to check intended 
substitution is actually correct.
* **backup**: Folder to store a backup folder tree with original links. This way you can keep a backup of your former
link files to restore then if anything goes wrong.

My advise is doing a first round using *--verbose*, *--simulate* and *backup* flags to be sure you are going to perform
the modification you really want, before actually touching anything, and to keep a fallback backup of your links:

````
C:\Users\User\relinker>./relinker --root="F:\Job\OneDrive - ACME\" --original="OneDrive - PreACME" --modified="Onedrive - ACME" -v -s --backup="C:\Users\User\Backup" 
````

After that, if you are really sure your links are going to be modified the way you want, you can run a modification round:
````
C:\Users\User\relinker>./relinker --root="F:\Job\OneDrive - ACME\" --original="OneDrive - PreACME" --modified="Onedrive - ACME" -v 
````

If something goes wrong or you realize modification was not what you intended, just copy your backup over your root folder. Backup
only contains link files at their proper locations. Just replace wrongly modified links with backup links.

## Afterwords:
That's all. If you find any bug feel free to fill a [issue](https://github.com/dante-signal31/relinker/issues), I'll try to fix the 
tool. You are welcome to ask for modifications or better offer a pull request.

Have a nice day!