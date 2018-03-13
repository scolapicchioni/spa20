<template>
<mdc-layout-grid>
  <mdc-layout-cell desktop=12 tablet=8>
    <form>
      <div>
        <mdc-textfield v-model="product.name" label="Product Name" />
      </div>
      <div>
        <mdc-textfield v-model="product.description" label="Product Description" multiline rows="8" cols="40" />
      </div>
      <div>
        <mdc-textfield v-model.number="product.price" label="Product Price" />
      </div>
      <div>
        <img ref="selectedImageElement"/>
        <input type="file" ref="fileUploadElement" @change="onFileChanged">
        <div>
          <mdc-button @click="fileUpload"><mdc-icon icon="file_upload"></mdc-icon>file upload</mdc-button>
        </div>
        <div class="camera-modal">
          <video ref="videoElement" class="camera-stream"/>
          <mdc-button @click="capture"><mdc-icon icon="camera"></mdc-icon>take picture</mdc-button>
        </div>
      </div>
      <mdc-button @click="insertProduct" raised>insert product</mdc-button>
    </form>
  </mdc-layout-cell>
</mdc-layout-grid>
</template>

<script>
import datalayer from '../datalayer'
export default {
  name: 'create-view',
  data () {
    return {
      product: {
        id: 0,
        name: '',
        description: '',
        price: 0
      },
      selectedFile: null,
      mediaStream: null
    }
  },
  async mounted () {
    const mediaStream = await navigator.mediaDevices.getUserMedia({ video: true })
    this.mediaStream = mediaStream
    this.$refs.videoElement.srcObject = mediaStream
    this.$refs.videoElement.play()
  },
  destroyed () {
    const tracks = this.mediaStream.getTracks()
    tracks.map(track => track.stop())
  },
  methods: {
    fileUpload () {
      this.$refs.fileUploadElement.click()
    },
    onFileChanged (event) {
      this.selectedFile = event.target.files[0]
      this.updateImage()
    },
    async capture () {
      const mediaStreamTrack = this.mediaStream.getVideoTracks()[0]
      const imageCapture = new window.ImageCapture(mediaStreamTrack)
      this.selectedFile = await imageCapture.takePhoto()
      this.updateImage()
    },
    updateImage () {
      const reader = new FileReader()
      reader.onload = () => { this.$refs.selectedImageElement.src = reader.result }
      reader.readAsDataURL(this.selectedFile)
    },
    async insertProduct () {
      const result = await datalayer.insertProduct(this.product, this.selectedFile)
      switch (result) {
        case 'Unauthorized':
          this.$root.$emit('show-snackbar', { message: result })
          break
        default:
          this.$router.push({name: 'HomeView'})
          break
      }
    }
  }
}
</script>

<style>
  input[type=file]{
    display: none;
  }
</style>