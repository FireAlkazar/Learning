class TreeNode<T> {
	value: T;
	left: TreeNode<T>;
	right: TreeNode<T>;
}

class BinaryTree {
	build(numbers: number[]) : TreeNode<number> {
		numbers.sort();
		return this.buildCore(numbers);
	}
	
	exists(node: TreeNode<number>, value: number) : boolean {
		if(node == null) {
			return false;
		}
	
		if(node.value == value) {
			return true;
		}
		
		if( node.value < value) {
			return this.exists(node.right, value);
		}
		else {
			return this.exists(node.left, value);
		}
	}
	
	private buildCore(numbers: number[]) : TreeNode<number> {
		if(numbers == null) {
			return null;
		}
		
		if(numbers.length == 1) {
			let node = new TreeNode<number>();
			node.value = numbers[0];
			return node;
		}
		
		var middleNumberIndex = Math.floor(numbers.length / 2);
		let middleNumber = numbers[middleNumberIndex];
		
		let node = new TreeNode<number>();
		node.value = middleNumber;
		node.left = this.buildCore(numbers.slice(0, middleNumberIndex));
		node.right = this.buildCore(numbers.slice(middleNumberIndex + 1, numbers.length));
		
		return node;
	}
}


let builder = new BinaryTree();
let node = builder.build([1,2,4,5,6,7,9]);
console.log(`1 - ${builder.exists(node,1)}`);
console.log(`3 - ${builder.exists(node,3)}`);
console.log(`5 - ${builder.exists(node,5)}`);
console.log(`6 - ${builder.exists(node,6)}`);
console.log(`10 - ${builder.exists(node,10)}`);


