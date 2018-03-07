# Images

We will add an image for each product.
We will cache the image both server side and client side.


## Wep Api
- Add ImageFile and ImageMimeType properties on Product model
- DataBaseInitializer
  - Update SeedData to accept a path and add ImageFile and MimeType
  - Add getImageFile 
- On Startup pass env.ContentRootPath
- Add Images folder with 4 images in it
- Add Migration
- Update DB
- Seed it

- ProductsController
  - Update CreateAsync to
    - Not look up frombody
    - Accept IFormFile as second parameter
    - Write the ImageMimeType and ImageFile of the product with the OpenStream().ReadAsync() of the image parameter

Client

- Both Home and Details:
  - Card Media
    - :src = [Data URI Scheme](https://en.wikipedia.org/wiki/Data_URI_scheme)
  - style { height: 50vh; background-size: 100% 100%;}
- Update datalayer.createAsync()
  - accept file as second parameter
  - create new FormData()
  - append each product property and file
  - remove content type https://stackoverflow.com/questions/36067767/how-do-i-upload-a-file-with-the-js-fetch-api

