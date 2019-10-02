# NOTE: THIS REPO IS OUTDATED. THE NEW VERSION USES VUE CLI 3.11 AND .NET CORE 3.0. REFER TO THE NEW REPO FOR THE NEW STEPS. https://github.com/scolapicchioni/spa30

# Images

In this lab we are going to add an image to each product.
We will give the user the possibility to either select an existing image from his device or to take a picture with his camera, using the [Media Devices API](https://developer.mozilla.org/en-US/docs/Web/API/MediaDevices) native functionality.
Server side we will modify our REST Service to send and receive files together with the Product data and we will store each file in our SQL DataBase.

## Showing existing Products images

We will first update the Model and DataBase server side, then we will proceed to update views on the client to show the picture on each product.

## Wep Api

We will start by modifying the server side.

### Modify the Model and DB structure by adding fields to hold the picture data

The info we need to store in the DB are an Array of Bytes for the picture and a String for the [MIME Type](https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types) (we will need the MIME type on the client to display the picture correctly).
We will also need to change the `DbInitializer.Initialize` to seed the `Products` table with pictures. 
We will add an `Images` folder on our server where we will save four `jpg`. Our `Initialize` will use the [ReadAllBytes](https://docs.microsoft.com/en-gb/dotnet/api/system.io.file.readallbytes?view=netcore-2.0) method of the `File` class to read the file from disk and write the bytes on the db. The `ReadAllBytes` method needs an absolute path. We will retrieve the [content root path](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.hosting.ihostingenvironment.contentrootpath?view=aspnetcore-2.0) in the `Startup` class and pass it as a parameter to the `Initialize` method.

We will take the following steps:
- Add `ImageFile` and `ImageMimeType` properties on `Product` model
- Add `Images` folder with 4 images in it
- On the `DbInitializer` class:
  - Update `Initialize` signature to accept a string `path`
  - Add `ImageFile` and `MimeType`
- On `Startup` class pass `env.ContentRootPath` as second parameter to the `Initialize`
- Add an `Entity Framework Migration`
- Update the DataBase
- Empty the `Products` table
- Seed the DB by running the application

In Visual Studio, open the `Models/Product.cs` file and add the following properties:

```cs
public byte[] ImageFile { get; set; }
public string ImageMimeType { get; set; }
```

Copy the `Labs\Lab08\Solution\MarketPlace\MarketPlace\MarketPlaceService\Images` folder to your `Labs\Lab08\Start\MarketPlace\MarketPlace\MarketPlaceService` folder. You will find four images inside it. We will use those four images to seed the `Products` table.

Open the `Data/DbInitializer.cs` file. Locate the `Initialize` method and change the signature from

```cs
public static void Initialize(MarketPlaceContext context) 
```

to

```cs
public static void Initialize(MarketPlaceContext context, string path) 
```

In the `Initialize` method, locate the following code

```cs
var products = new Product[] {
  new Product { Name = "Product 1", Description = "First Sample Product", Price = 1234 , UserName = "alice@gmail.com"},
  new Product { Name = "Product 2", Description = "Lorem Ipsum", Price = 555 , UserName = "bob@gmail.com"},
  new Product { Name = "Product 3", Description = "Third Sample Product", Price = 333 , UserName = "alice@alice.com"},
  new Product { Name = "Product 4", Description = "Fourth Sample Product", Price = 44 , UserName = "alice@gmail.com"}
};
```

and replace it with

```cs
var products = new Product[] {
  new Product { Name = "Product 1", Description = "First Sample Product", Price = 1234 , UserName = "alice@gmail.com",
      ImageFile = File.ReadAllBytes($@"{path}\Images\flower.jpg"), ImageMimeType = "image/jpeg"},
  new Product { Name = "Product 2", Description = "Lorem Ipsum", Price = 555 , UserName = "bob@gmail.com",
      ImageFile = File.ReadAllBytes($@"{path}\Images\orchard.jpg"), ImageMimeType = "image/jpeg"},
  new Product { Name = "Product 3", Description = "Third Sample Product", Price = 333 , UserName = "alice@alice.com",
      ImageFile = File.ReadAllBytes($@"{path}\Images\path.jpg"), ImageMimeType = "image/jpeg"},
  new Product { Name = "Product 4", Description = "Fourth Sample Product", Price = 44 , UserName = "alice@gmail.com",
      ImageFile = File.ReadAllBytes($@"{path}\Images\blackberries.jpg"), ImageMimeType = "image/jpeg"}
};
```

Open the `Startup.cs` file, locate the `Configure` method and replace the following code

```cs
DbInitializer.Initialize(context);
```

with 

```cs
DbInitializer.Initialize(context, env.ContentRootPath);
```

Open the `Package Manager Console` and type the following commands:

```
Add-Migration "ProductImageFile"
Update-DataBase
```

Open your `SQL Server Object Explorer`, locate your `MarketPlace` database, open the `Products` table, select all the products and delete them.

Run the application and verify that the `Products` table now contains four products with bytes for each product.

Let's proceed to show the images on our client application.

## The Client

Seen the fact that we use the Vue MDC Adapter [Card](https://stasson.github.io/vue-mdc-adapter/#/component/card) component to show a product, we can use a `mdc-card-media` element to show the picture:
> Media area that displays a custom background-image with background-size: cover

We will dynamically bind the `src` attribute to the `product.imageMimeType` and `product.imageFile` properties by using a [Data URI Scheme](https://en.wikipedia.org/wiki/Data_URI_scheme)

Open the `src/components/HomeView.vue` file and add the following code as first element of the `<mdc-card>`, before the `<mdc-card-header>` element:

```html
<mdc-card-media :src="'data:' + product.imageMimeType + ';base64,' + product.imageFile" />
``` 

Do the same for the `src/components/DetailsView.vue` file.

Run the application and verify that the images are shown on each card.
You may notice that the picture gets cut. In order to change this we can apply a style to the `.mdc-card-media` class. We can for example increase the `height` of the media card to 50% of the [ViewPort Height](https://www.sitepoint.com/css-viewport-units-quick-start/) and the scale the [background-size](https://www.w3schools.com/cssref/css3_pr_background-size.asp) to 100% height and 100% width.

Open your `src/components/HomeView.vue`, locate the `<style>` tag and replace it with:

```css
<style>
.mdc-card-media  {
  height: 50vh;
  background-size: 100% 100%;
}
</style>
```

You should now see each card containing a bigger picture.

**Note: If you want to use the exact same stylesheet rules on the `DetailsView`, you don't need to update the style there too, because the `<style>` tag is applied globally. Should you want to apply a different style, remember to use the [scoped](https://vue-loader.vuejs.org/en/features/scoped-css.html) attribute on the style tag**

## Product Create - Now with pictures!

It's time for us to tackle the product picture upload. We will need to update once again both the server and the client side.

### Client side : select a file from hard disk

Our final goal is to give the user the possibility to either chose the file from hard disk or to snap a picture with the camera, but we will start by implementing the first option.

To let the user select a file from hard disk, we can use a FileUpload html element, as explained [here](https://developer.mozilla.org/en-US/docs/Web/API/File/Using_files_from_web_applications).

On the `CreateView.vue`, we will:
- Add a `file`, `button` and `image` element
- Add a `selectedFile` property to the object returned by the `data` function
- Handle the `onchange` event of the `file` element in an `onFileChanged` method that updates the `selectedFile` with the first selected file and refreshes the `src` property of the `image` element by using a `FileReader`  
- Hide the `file` element and invoke the `click` from a button
- Update the `insertProduct` to pass the `selectedFile` as second parameter

We are going to reference the `file` and `image` elements from our script; in order to do that, we will take advantage of the [refs](https://codingexplained.com/coding/front-end/vue-js/accessing-dom-refs) feature of Vue.

Open the `src/components/CreateView.vue` file, locate the textfield for the product price and add the following code:

```html
<div>
  <img ref="selectedImageElement"/>
  <input type="file" ref="fileUploadElement" @change="onFileChanged">
  <div>
    <mdc-button @click="fileUpload"><mdc-icon icon="file_upload"></mdc-icon>file upload</mdc-button>
  </div>
</div>
```

Now locate the `data` function in the `<script>` tag and add a `selectedFile` property, as follows:

```js
data () {
  return {
    product: {
      id: 0,
      name: '',
      description: '',
      price: 0
    },
    selectedFile: null
  }
}
```

Now add the `onFileChanged` method under the `methods` property of your component:

```js
onFileChanged (event) {
  this.selectedFile = event.target.files[0]
  this.updateImage()
}
```

Let's also add an `updatemage` method, whose job is to read the file by using the `FileReader` and update the `src` property of our `image` element:

```js
updateImage () {
  const reader = new FileReader()
  reader.onload = () => { this.$refs.selectedImageElement.src = reader.result }
  reader.readAsDataURL(this.selectedFile)
}
```

We also need a `fileUpload` method to handle the click of the button:

```js
fileUpload () {
  this.$refs.fileUploadElement.click()
}
```

Now we need to update the `insertProduct` method to pass our `selectedFile` as a second parameter. Locate the `insertProductMethod` and replace this line of code

```js
const result = await datalayer.insertProduct(this.product)
```

with this:

```js
const result = await datalayer.insertProduct(this.product, this.selectedFile)
```

The last task is to hide the `file` element. Locate the `<style>` tag and replace it with the following code:

```css
<style>
  input[type=file]{
    display: none;
  }
</style>
```

If you run the application now, you should be able to go to the `CreateView`, select a file from the hard disk by clicking on the `File Upload` button and you should see the image appearing on the page.

The datalayer does not send the file to the server yet. Let's fix that.

So far, we have serialized the product as JSON in order to send it to the server. Now that we also have to send a picture, we will use a different approach instead. We will simulate a `form` by appending all the product properties and the file into a `FormData` object, as described [here](https://stackoverflow.com/questions/36067767/how-do-i-upload-a-file-with-the-js-fetch-api)
We will also remove the `Content-type` header so that the server knows that we're not sending `json` anymore.

Open the `datalayer.js` file and replace the `insertProduct` with the follwing code:

```js
async insertProduct (product, file) {
  const user = await applicationUserManager.getUser()
  product.userName = user.profile.email
  const data = new FormData()
  data.append('file', file)
  for (const key in product) {
    if (product.hasOwnProperty(key)) {
      const value = product[key]
      data.append(key, value)
    }
  }
  const response = await fetch(this.serviceUrl, {
    method: 'POST',
    body: data,
    headers: new Headers({
      'Authorization': 'Bearer ' + (user ? user.access_token : '')
    })
  })
  let result
  if (response.status !== 201) {
    result = response.statusText
  } else {
    result = await response.json()
  }
  return result
}
```

Our client is ready, but the insert operation does not work because the server does not accept and save the file yet. Let's fix the `MarketPlace` project.

## Web API

The individual files uploaded to the server can be accessed through Model Binding using the IFormFile interface, as described [here](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads).

What we're going to do is to update our `CreateAsync` method of the `ProductsController`: 
- The signature will have to accept a second parameter `file` of type `IFormFile`
- The method will 
  - Set the `product.ImageMimeType` property to the value of the `file.ContentType`
  - Copy the file to a `MemoryStream`
  - Set the `product.ImageFile` property to the `memoryStream.ToArray` value

Open your `Controllers/ProductController.cs` file, locate the `CreateAsync` method and replace it with the following code:

```cs
public async Task<IActionResult> CreateAsync(Product product, IFormFile file) {
  if (product == null || product.UserName != User.Identity.Name) {
      return BadRequest();
  }

  if (file != null) {
      product.ImageMimeType = file.ContentType;

      using (var memoryStream = new MemoryStream()) {
          await file.CopyToAsync(memoryStream);
          product.ImageFile = memoryStream.ToArray();
      }
  }

  await _ProductsRepository.AddAsync(product);

  return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
}
```

If you run the application you should now be able to upload a picture from file.

The next step is to update the client so that the user can take a picture with the device camera.

We're going to use two APIs: [MediaDevices](https://developer.mozilla.org/en-US/docs/Web/API/MediaDevices) and [ImageCapture](https://developer.mozilla.org/en-US/docs/Web/API/ImageCapture).

There are many examples that we can use to understand how it works:
- https://developer.mozilla.org/en-US/docs/Web/API/MediaStream_Image_Capture_API
- https://googlechrome.github.io/samples/image-capture/grab-frame-take-photo.html

We are going to follow this [tutorial](https://medium.com/theodo/a-progressive-web-application-with-vue-js-webpack-material-design-part-4-96c8c216810b) that is specific for VueJs.

We will: 
- Add a `video` and `button` elements in our template
- Handle: 
  - The `mounted` event to 
    - get the video MediaStream
    - link it to the `video` element 
    - play the video so that the user can see the camera stream on the page
  - The `click` of the `button` to 
    - create an `ImageCapture` from the `mediaStreamTrack`
    - take a photo
    - update the `selectedFile` and the `image` element
  - The `destroyed` event to stop the stream when the user exits the page

  On the `src/components/CreateView.vue` file, locate the `file` element and add the following code:

  ```html
  <div class="camera-modal">
    <video ref="videoElement" class="camera-stream"/>
    <mdc-button @click="capture"><mdc-icon icon="camera"></mdc-icon>take picture</mdc-button>
  </div>
  ``` 

  In the `<script>` tag, hook the `mounted` event to the following code:

  ```js
  async mounted () {
    const mediaStream = await navigator.mediaDevices.getUserMedia({ video: true })
    this.mediaStream = mediaStream
    this.$refs.videoElement.srcObject = mediaStream
    this.$refs.videoElement.play()
  }
  ```

  Then handle the `destroyed` event with this code:

  ```js
  destroyed () {
    const tracks = this.mediaStream.getTracks()
    tracks.map(track => track.stop())
  }
  ```

  And finally add the following method in the `methods` section:

  ```js
  async capture () {
    const mediaStreamTrack = this.mediaStream.getVideoTracks()[0]
    const imageCapture = new window.ImageCapture(mediaStreamTrack)
    this.selectedFile = await imageCapture.takePhoto()
    this.updateImage()
  }
  ```

  Because the `Blob` returned by the `takePhoto` has the same interface as the `File` returned by the `file` element, we don't need to add anything else. If you run the application you should be able to see the camera stream, take a picture and upload it with a new product.
  
