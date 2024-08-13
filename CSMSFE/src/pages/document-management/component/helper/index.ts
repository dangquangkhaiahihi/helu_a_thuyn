import { DocumentDTO } from "@/common/DTO/Document/DocumentDTO";

export function getNodeAddress(treeData: DocumentDTO, path: number[]): string | null {
    const helper = (node: DocumentDTO, remainingPath: number[], address: string[]): string | null => {
        if (remainingPath.length === 0) {
            return address.join(' / ');
        }

        const childIndex = remainingPath[0];
        if (!node.children || childIndex >= node.children.length) {
            return ""; // Path is invalid
        }

        const nextNode = node.children[childIndex];
        
        return helper(nextNode, remainingPath.slice(1), [...address, nextNode.name]);
    };

    return helper(treeData, path, [treeData.name]);
}

export function getFileNameWithoutExtension(fileName: string) {
    const parts = fileName.split('.');
    parts.pop(); // Remove the last element (the extension)
    return parts.join('.'); // Join the remaining parts back together
}