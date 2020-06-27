var app = new Vue({
    el: '#app',
    data: {
        editing: false,
        loading: false,
        objectIndex: 0,
        productModel: {
            id: 0,
            name: "Product Name",
            description: "Description",
            value: 1999.98
        },
        products: []
    },
    mounted() {
        this.getProducts();
    },
    methods: {
        getProducts() {
            this.loading = true;
            axios.get('/products')
                .then(res => {
                    console.log(res);
                    this.products = res.data;
                })
                .catch(err => {
                    console.log(err.response);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        getProduct(id) {
            this.loading = true;
            axios.get('/products/' + id)
                .then(res => {
                    console.log(res);
                    var product = res.data;
                    this.productModel = {
                        id: product.id,
                        name: product.name,
                        description: product.description,
                        value: product.value
                    };
                })
                .catch(err => {
                    console.log(err.response);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        createProduct() {
            if (this.productModel.name != ""
                && this.productModel.description != ""
                && this.productModel.value != "") {
                this.loading = true;
                axios.post('/products', this.productModel)
                    .then(res => {
                        this.products.push(res.data);
                    })
                    .catch(err => {
                        console.log(err.response);
                    })
                    .then(() => {
                        this.loading = false;
                        this.editing = false;
                    });
            } else {
                alert('Заполните все поля');
            }
        },
        updateProduct(product) {
            if (this.productModel.name != ""
                && this.productModel.description != ""
                && this.productModel.value != "") {
                this.loading = true;
                axios.put('/products', this.productModel)
                    .then(res => {
                        console.log(res.data);
                        this.products.splice(this.objectIndex, 1, res.data);
                    })
                    .catch(err => {
                        console.log(err.response);
                    })
                    .then(() => {
                        this.loading = false;
                        this.editing = false;
                    });
            } else {
                alert('Заполните все поля');
            }
        },
        editProduct(id, index) {
            this.objectIndex = index;
            this.getProduct(id);
            this.editing = true;
        },
        deleteProduct(id, index) {
            this.loading = true;
            axios.delete('/products/' + id)
                .then(res => {
                    console.log(res);
                    this.products.splice(index, 1);
                })
                .catch(err => {
                    console.log(err.response);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        newProduct() {
            this.editing = true;
            this.productModel.id = 0;
        },
        cancel() {
            this.editing = false;
        }
    },
    computed: {
    }
});