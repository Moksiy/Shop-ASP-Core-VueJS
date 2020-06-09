var app = new Vue({
    el: '#app',
    data: {
        loading: false,
        productModel: {
            name: "Product Name",
            description: "Description",
            value: 1999.99
        },
        products: []
    },
    methods: {
        getProducts() {
            this.loading = true;
            axios.get('/Admin/products')
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
        createProduct() {
            this.loading = true;
            axios.post('/Admin/products', this.productModel)
                .then(res => {
                    console.log(res.data);
                    this.products.push(res.data);
                })
                .catch(err => {
                    console.log(err.response);
                })
                .then(() => {
                    this.loading = false;
                });
        }
    },
    computed: {
    }
})