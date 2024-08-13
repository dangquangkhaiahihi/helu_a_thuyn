import { Model } from "@/common/DTO/Model/ModelDTO";

export function organizeDataIntoTree(data: Model[]): Promise<Model[]> {
    return new Promise((resolve) => {
        const map: { [key: string]: Model } = {};
        const roots: Model[] = [];

        // Initialize map with nodes, children, and path
        data.forEach(item => {
            map[item.id] = { ...item, children: [], path: [] };
        });

        // Build the tree structure
        data.forEach(item => {
            const node = map[item.id];
            if (item.parentId) {
                const parentNode = map[item.parentId];
                if (parentNode) {
                    parentNode.children!.push(node);
                }
            } else {
                roots.push(node);
            }
        });

        // Update paths
        function updatePaths(node: Model, path: number[]) {
            node.path = path;
            node.children?.forEach((child, index) => {
                updatePaths(child, [...path, index]);
            });
        }

        // Start path update from each root node
        roots.forEach((root, index) => {
            updatePaths(root, [index]);
        });

        resolve(roots);
    });
}

export function flattenTree(nodes: Model[]): Promise<Model[]> {
    return new Promise((resolve) => {
        const result: Model[] = [];
        const flatten = (node: Model) => {
            result.push(node);
            if (node.children) {
                node.children.forEach(child => flatten(child));
            }
        };
    
        nodes.forEach(node => flatten(node));
    
        resolve(result);
    });
}

// Function to add a new node to the data structure
export function addNode(data: Model[], path: number[], newNode: Model): Promise<Model[]> {
    return new Promise((resolve) => {
        // Helper function to add the new node at the specified path
        const addNodeAtPath = (nodes: Model[], currentPath: number[], newNode: Model, parentPath: number[]): Model[] => {
            if (currentPath.length === 0) {
                // Add the new node to the current level
                const newPath = [...parentPath, nodes.length];
                return [...nodes, { ...newNode, path: newPath }];
            }
        
            // Recursively navigate to the specified path
            const [currentIndex, ...restPath] = currentPath;
            return nodes.map((node, index) => {
                if (index === currentIndex) {
                    // If it's the direct parent and its type is "MODEL", change it to "FOLDER"
                    if (restPath.length === 0 && node.type === "MODEL") {
                        node.type = "FOLDER";
                    }
                    return {
                        ...node,
                        children: addNodeAtPath(node.children || [], restPath, newNode, [...parentPath, currentIndex])
                    };
                }
                return node;
            });
        };
      
        // Đoạn này để chỉ chạy 1 mình FE demo
        // // Get the parent node's ID based on the path
        // const getParentId = (nodes: Model[], path: number[]): string | null => {
        //   let currentNodes = nodes;
        //   let parentId: string | null = null;
    
        //   for (let i = 0; i < path.length; i++) {
        //     const currentIndex = path[i];
        //     if (currentNodes[currentIndex]) {
        //       parentId = currentNodes[currentIndex].id;
        //       currentNodes = currentNodes[currentIndex].children || [];
        //     } else {
        //       return null;
        //     }
        //   }
    
        //   return parentId;
        // };
    
        // // Create the new node
        // const newNode: Model = {
        //     id: newId,
        //     name: newNodeName,
        //     level: path.length + 1,
        //     description: '',
        //     speckleBranchId: '',
        //     type: 'MODEL',
        //     projectId: '',
        //     projectName: '',
        //     status: '',
        //     parentId: getParentId(data, path),
        //     createdBy: '',
        //     createdDate: new Date().toISOString(),
        //     modifiedBy: '',
        //     modifiedDate: new Date().toISOString(),
        //     children: [],
        //     path: []
        // };
  
        // Add the new node to the data structure and resolve the promise with the result
        const result = addNodeAtPath(data, path, newNode, []);
        resolve(result);
    });
}

export function updateNode(data: Model[], path: number[], newName: string): Promise<Model[]> {
    return new Promise((resolve) => {
        // Helper function to update the node's name at the specified path
        const updateNodeAtPath = (nodes: Model[], currentPath: number[]): Model[] => {
            if (currentPath.length === 0) {
                return nodes;
            }
        
            const [currentIndex, ...restPath] = currentPath;
            return nodes.map((node, index) => {
                if (index === currentIndex) {
                    if (restPath.length === 0) {
                        // Update the node's name if we reached the target node
                        return {
                            ...node,
                            name: newName
                        };
                    }
                    return {
                        ...node,
                        children: updateNodeAtPath(node.children || [], restPath)
                    };
                }
                return node;
            });
        };
      
        // Update the node's name at the specified path
        const result = updateNodeAtPath(data, path);
        resolve(result);
    });
}
  
export function deleteNode(data: Model[], path: number[]): Promise<Model[]> {
    return new Promise((resolve) => {
        // Helper function to delete the node at the specified path
        const deleteNodeAtPath = (nodes: Model[], currentPath: number[]): Model[] => {
            if (currentPath.length === 0) {
                return nodes;
            }

            const [currentIndex, ...restPath] = currentPath;

            return nodes
                .map((node, index) => {
                    if (index === currentIndex) {
                        if (restPath.length === 0) {
                            // If we reached the target node, skip it from the result array (effectively deleting it)
                            return null;
                        }
                        const updatedChildren = deleteNodeAtPath(node.children || [], restPath);

                        // Check if the current node's children array will have only one element before deletion
                        if (node.children && node.children.length === 1 && updatedChildren.length === 0) {
                            return {
                                ...node,
                                children: updatedChildren,
                                type: 'MODEL',
                                isUpload: false
                            };
                        }

                        return {
                            ...node,
                            children: updatedChildren
                        };
                    }
                    return node;
                })
                .filter(node => node !== null) as Model[];
        };

        // Perform the deletion and resolve the promise with the result
        const result = deleteNodeAtPath(data, path);
        resolve(result);
    });
}

export function moveNode(dataNormal: Model[], sourceNodeId: string, destinationNodeId: string): Promise<Model[]> {
    return new Promise((resolve, reject) => {
        // Create a map to easily access nodes by their ID
        const nodeMap = new Map<string, Model>();
        const parentMap = new Map<string, Model>();

        // Populate the maps and find source and destination nodes
        dataNormal.forEach((item) => {
            const node = { ...item };  // Clone the item to avoid direct mutations
            nodeMap.set(node.id, node);
            if (node.parentId) {
                parentMap.set(node.parentId, node);
            }
        });

        // Retrieve source and destination nodes
        const sourceNode = nodeMap.get(sourceNodeId);
        const destinationNode = destinationNodeId ? nodeMap.get(destinationNodeId) : null;

        // Check if source node is found
        if (!sourceNode) {
            console.error("Source node not found.");
            reject("Source node not found.");
            return;
        }

        if (destinationNodeId && !destinationNode) {
            console.error("Destination node not found.");
            reject("Destination node not found.");
            return;
        }

        // Determine sourceNodeParent
        const sourceNodeParent = sourceNode.parentId ? nodeMap.get(sourceNode.parentId) : null;

        if (sourceNodeParent) {
            // Count children of sourceNodeParent
            const countChildren = [...nodeMap.values()].filter(item => item.parentId === sourceNodeParent.id).length;

            // Update parent node type if it only has one child (the source node)
            if (countChildren === 1) {
                sourceNodeParent.type = "MODEL";
                sourceNodeParent.isUpload = false;
            }
        }

        // Check if destination node can accept the move
        if (destinationNode && destinationNode.type === "MODEL" && destinationNode.isUpload) {
            console.error("Cannot move node to destination as it is marked for upload.");
            reject("Cannot move node to destination as it is marked for upload.");
            return;
        } else if (destinationNode && destinationNode.type === "MODEL") {
            destinationNode.type = "FOLDER";
            destinationNode.isUpload = false;
        }

        // Move the source node
        sourceNode.parentId = destinationNodeId || null;

        // Update paths for the source node and its children
        function updatePath(node: Model, parentPath: number[]): void {
            node.path = [...parentPath, node.level];
            if (node.children) {
                node.children.forEach(child => updatePath(child, node.path));
            }
        }

        if (!destinationNodeId) {
            // Move to first level
            sourceNode.path = [sourceNode.level];
            sourceNode.level = 1;
            if (sourceNode.children) {
                sourceNode.children.forEach(child => updatePath(child, sourceNode.path));
            }
        } else if (destinationNode) {
            // Move under the destination node
            updatePath(sourceNode, destinationNode.path);
        }

        // Rebuild the data structure and update paths
        const newData = Array.from(nodeMap.values());
        organizeDataIntoTree(newData).then(result => {
            console.log("result", result);
            resolve(result);
        });
    });
}

export function updateTreeAfterUploadModel( dataNormal: Model[], sourceNodeId: string ): Promise<Model[]> {
    return new Promise((resolve, reject) => {
        let isFound = false;
        dataNormal.forEach(item => {
            if ( item.id === sourceNodeId ) {
                isFound = true;
                item.isUpload = true;
                return;
            }
        });

        if ( !isFound ) reject("Source node not found.");

        organizeDataIntoTree(dataNormal).then(result => {
            resolve(result);
        });
    });
}
