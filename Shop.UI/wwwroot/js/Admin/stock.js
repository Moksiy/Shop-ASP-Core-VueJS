var app = new Vue({
    el: '#app',
    data: {
        products: [],
        selectedProduct: null,
        flag: false,
        newStock: {
            productId: 0,
            description: "Bla-Bla-Bla",
            qty: 10
        }
    },
    mounted() {
        this.getStock();
    },
    methods: {
        getStock() {
            this.loading = true;
            axios.get('/stocks')
                .then(res => {
                    this.products = res.data;
                })
                .catch(err => {
                    console.log(err.response);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        updateStock() {
            this.loading = true;
            flag = false;

            this.selectedProduct.stock.map(x => {
                if (x.description == "" || String(x.qty).length == 0) {
                    flag = true;
                }
            });
            if (!flag) {
                axios.put('/stocks', {
                    stock: this.selectedProduct.stock.map(x => {
                        return {
                            id: x.id,
                            description: x.description,
                            qty: x.qty,
                            productId: this.selectedProduct.id
                        };
                    })
                })
                    .then(res => {
                        this.selectedProduct.stock.splice(index, 1);
                    })
                    .catch(err => {
                        console.log(err.response);
                    })
                    .then(() => {
                        this.loading = false;
                    });
            } else {
                alert('Не все поля заполнены');
            }
        },
        deleteStock(id, index) {
            this.loading = true;
            axios.delete('/stocks/' + id)
                .then(res => {
                    this.selectedProduct.stock.splice(index, 1);
                })
                .catch(err => {
                    console.log(err.response);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        addStock() {
            if (this.newStock.description != ""
                && this.newStock.qty != "") {
                this.loading = true;
                axios.post('/stocks', this.newStock)
                    .then(res => {
                        this.selectedProduct.stock.push(res.data);
                    })
                    .catch(err => {
                        console.log(err.response);
                    })
                    .then(() => {
                        this.loading = false;
                    });
            } else {
                alert('Заполните все поля');
            }
        },
        selectProduct(product) {
            this.selectedProduct = product;
            this.newStock.productId = product.id;
        }
    }
})