import { useRef, useEffect, useState } from 'react';
import useTreeState, {
    findTargetPathByProp,
	findAllTargetPathByProp,
    findTargetNode,
} from "use-tree-state";

function useTreeStateCustom({ initialTreeState }) {
	const { treeState, reducers } = useTreeState({ data: initialTreeState });

	return {
		addNode: reducers.addNode,
		addNodeByProp: reducers.addNodeByProp,
		checkNode: reducers.checkNode,
		checkNodeByProp: reducers.checkNodeByProp,
		deleteNode: reducers.deleteNode,
		deleteNodeByProp: reducers.deleteNodeByProp,
		renameNode: reducers.renameNode,
		renameNodeByProp: reducers.renameNodeByProp,
		setTreeState: reducers.setTreeState,
		toggleOpen: reducers.toggleOpen,
		toggleOpenByProp: reducers.toggleOpenByProp,
		findTargetPathByProp,
		findAllTargetPathByProp,
		findTargetNode,
		treeState
	}
}

export default useTreeStateCustom;
