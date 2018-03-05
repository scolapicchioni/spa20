const datalayer = {
  products: [
    {id: 1, name: 'WIN-WIN survival strategies', description: 'Bring to the table win-win survival strategies to ensure proactive domination.', price: 12345},
    {id: 2, name: 'HIGH level overviews', description: 'Iterative approaches to corporate strategy foster collaborative thinking to further the overall value proposition.', price: 2345},
    {id: 3, name: 'ORGANICALLY grow world', description: 'Organically grow the holistic world view of disruptive innovation via workplace diversity and empowerment.', price: 45678},
    {id: 4, name: 'AGILE frameworks', description: 'Leverage agile frameworks to provide a robust synopsis for high level overviews', price: 9876}
  ],
  getProducts () {
    return this.products
  },
  getProductById (id) {
    return this.products.find(p => p.id === id)
  },
  insertProduct (product) {
    const id = this.products.reduce((prev, curr) => prev.id > curr.id ? prev.id : curr.id) + 1
    product.id = id
    this.products.push(product)
  },
  updateProduct (id, product) {
    const oldProduct = this.products.find(p => p === id)
    if (oldProduct) {
      oldProduct.name = product.name
      oldProduct.price = product.price
      oldProduct.description = product.description
    }
  },
  deleteProduct (id) {
    const productindex = this.products.findIndex(p => p.id === id)
    this.products.splice(productindex, 1)
  }
}

export default datalayer
