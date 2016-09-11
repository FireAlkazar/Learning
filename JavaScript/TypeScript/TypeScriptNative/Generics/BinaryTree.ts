class TreeNode<T> {
	value: T;
	left: TreeNode<T>;
	right: TreeNode<T>;
}

class BinaryTree {
	arrayToTree(numbers: number[]) : TreeNode<number> {
		numbers.sort();
		return this.buildCore(numbers);
	}
	
	treeToArray(node: TreeNode<number>) : number[] {
		if(node == null) {
			return [];
		}
		
		let left = this.treeToArray(node.left);
		let value = node.value;
		let right = this.treeToArray(node.right);
		
		left.push(value);
		for(let i=0; i<right.length;i++) {
			left.push(right[i]);
		}
		
		return left; 
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


let binaryTree = new BinaryTree();
let originalArray = [1,2,4,5,6,7,9];
console.log(`original array - ${originalArray}`);
let tree = binaryTree.arrayToTree(originalArray);
console.log(`array from tree - ${binaryTree.treeToArray(tree)}`);


