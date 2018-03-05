const datalayer = {
  serviceUrl: 'http://localhost:5000/api/products',
  async getProducts () {
    const response = await fetch(this.serviceUrl)
    return response.json()
  },

  async getProductById (id) {
    const response = await fetch(`${this.serviceUrl}/${id}`)
    return response.json()
  },

  async insertProduct (product) {
    const response = await fetch(this.serviceUrl, {
      method: 'POST',
      body: JSON.stringify(product),
      headers: new Headers({
        'Content-Type': 'application/json'
      })
    })
    return response.json()
  },

  async updateProduct (id, product) {
    return fetch(`${this.serviceUrl}/${id}`, {
      method: 'PUT',
      body: JSON.stringify(product),
      headers: new Headers({
        'Content-Type': 'application/json'
      })
    })
  },

  async deleteProduct (id) {
    return fetch(`${this.serviceUrl}/${id}`, {
      method: 'DELETE'
    })
  }
}

export default datalayer
