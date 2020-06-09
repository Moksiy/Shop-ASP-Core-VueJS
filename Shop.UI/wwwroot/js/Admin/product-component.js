Vue.component('product-manager', {
    template: `
<div v-if="!editing">
                <button class="button" @click="newProduct">Добавить товар</button>
                <table class="table">
                    <tr>
                        <th>ID</th>
                        <th>Товар</th>
                        <th>Цена</th>
                        <th></th>
                        <th></th>
                    </tr>
                    <tr v-for="(product, index) in products">
                        <td>{{product.id}}</td>
                        <td>{{product.name}}</td>
                        <td>{{product.value}}</td>
                        <td><a @click="editProduct(product.id, index)">Редактировать</a></td>
                        <td><a @click="deleteProduct(product.id, index)">Удалить</a></td>
                    </tr>
                </table>
            </div>

            <div v-else>
                <div class="field">
                    <label class="label">Название товара</label>
                    <div class="control">
                        <input class="input" v-model="productModel.name" />
                    </div>
                </div>

                <div class="field">
                    <label class="label">Описание товара</label>
                    <div class="control">
                        <input class="input" v-model="productModel.description" />
                    </div>
                </div>

                <div class="field">
                    <label class="label">Цена товара</label>
                    <div class="control">
                        <input class="input" v-model="productModel.value" />
                    </div>
                </div>

                <button class="button is-success" @click="createProduct" v-if="!productModel.id">Добавить товар</button>
                <button class="button is-warning" @click="updateProduct" v-else>Редактировать</button>
                <button class="button" @click="cancel">Выход</button>
            </div>
        </div>`,
    data() {
        return {
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
        }
    },
    mounted() {
        this.getProducts();
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
        getProduct(id) {
            this.loading = true;
            axios.get('/Admin/products/' + id)
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
                    this.editing = false;
                });
        },
        updateProduct(product) {
            this.loading = true;
            axios.put('/Admin/products', this.productModel)
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
        },
        editProduct(id, index) {
            this.objectIndex = index;
            this.getProduct(id);
            this.editing = true;
        },
        deleteProduct(id, index) {
            this.loading = true;
            axios.delete('/Admin/products/' + id)
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