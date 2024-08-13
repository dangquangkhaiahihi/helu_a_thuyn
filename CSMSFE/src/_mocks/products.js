function createData(id, productImg, name) {
	return {
		id,
		productImg,
		name,
	};
}

export default [
	createData(1, '/assets/images/products/product_1.jpg', 'The Dothraki Shoes'),
	createData(2, '/assets/images/products/product_2.jpg', 'Selonian Hand Bag'),
	createData(3, '/assets/images/products/product_3.jpg', 'Kubaz Sunglass'),
	createData(4, '/assets/images/products/product_4.jpg', 'Kel Dor Sunglass'),
	createData(5, '/assets/images/products/product_5.jpg', 'Westeros Sneaker'),
];
